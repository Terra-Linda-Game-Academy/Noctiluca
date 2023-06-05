using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Potions.Fluids;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Potions {
    [RequireComponent(typeof(BoxCollider), typeof(DecalProjector))]
    //[ExecuteInEditMode]
    public class Puddle : MonoBehaviour {
        private class PointMetadata {
            public readonly float Lifetime;
            private readonly float initialSize;
            private float secondsActive;
            public float SecondsActive => secondsActive;
            public Point Point { get; private set; }

            public bool IsActive { get; private set; }


            public PointMetadata(Fluid fluid, Vector2 position) {
                Lifetime = fluid.InitialLifetime;
                initialSize = fluid.InitialSize;
                Point = new Point(
                    fluid.PrimaryColor.Evaluate(0),
                    fluid.SecondaryColor.Evaluate(0), 
                    position,
                    initialSize
                );
                secondsActive = 0;
                IsActive = true;
            }

            public void Update(Fluid fluid) {
                secondsActive += Time.deltaTime;
                var lifetimeProgress = fluid.LifeProgress(Lifetime, secondsActive);

                Color primary = fluid.PrimaryColor.Evaluate(lifetimeProgress);
                Color secondary = fluid.SecondaryColor.Evaluate(lifetimeProgress);
                
                Point = new Point(primary, secondary, Point.Pos, initialSize * (1 - lifetimeProgress));

                if (secondsActive >= Lifetime) { IsActive = false; }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Point {
            private float size;
            public float Size => size;

            private readonly ushort x;
            private readonly ushort z;
            public Vector2 Pos => new Vector2(Mathf.HalfToFloat(x), Mathf.HalfToFloat(z));

            private ushort r1;
            private ushort g1;
            private ushort b1;

            public Color PrimaryColor {
                private set {
                    r1 = Mathf.FloatToHalf(value.r);
                    g1 = Mathf.FloatToHalf(value.g);
                    b1 = Mathf.FloatToHalf(value.b);
                }
                get => new(Mathf.HalfToFloat(r1), Mathf.HalfToFloat(g1), Mathf.HalfToFloat(b1));
            }

            private ushort r2;
            private ushort g2;
            private ushort b2;

            private Color SecondaryColor {
                set {
                    r2 = Mathf.FloatToHalf(value.r);
                    g2 = Mathf.FloatToHalf(value.g);
                    b2 = Mathf.FloatToHalf(value.b);
                }
            }

            public Point(Color primary, Color secondary, Vector2 position, float size) {
                x = Mathf.FloatToHalf(position.x);
                z = Mathf.FloatToHalf(position.y);
                this.size = size;

                r1 = g1 = b1 = 0;
                r2 = g2 = b2 = 0;

                PrimaryColor = primary;
                SecondaryColor = secondary;
            }
        }

        private Mesh cubeMesh;
        private Material puddleMaterial;
        private DecalProjector projector;
        private ComputeBuffer pointsBuffer;
        private BoxCollider boxCollider;
        private Fluid fluid;
        public Fluid Fluid {
            get => fluid;
            set {
                if (value == fluid) return;
                fluid = value;
                ResetFluid();
            }
        }

        private List<PointMetadata> points;
        private SortedSet<float> xPoints;
        private SortedSet<float> zPoints;
        private float minY;
        private float maxY;

        private Dictionary<GameObject, float> cooldowns;

        private Vector3 Scale =>
            new Vector3(xPoints.Max - xPoints.Min, Mathf.Max(maxY - minY, 1), zPoints.Max - zPoints.Min);

        private Vector3 Min => new Vector3(xPoints.Min, minY, zPoints.Min);

        private Vector3 Center =>
            new Vector3(
                (xPoints.Min + xPoints.Max) / 2,
                (minY + maxY) / 2,
                (xPoints.Min + zPoints.Max) / 2
            );

        private void Awake() {
            cubeMesh = CoreUtils.CreateCubeMesh(Vector3.zero, Vector3.one);
            puddleMaterial = CoreUtils.CreateEngineMaterial("Hidden/FixedPuddle");
            boxCollider = GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            projector = GetComponent<DecalProjector>();
            projector.material = puddleMaterial;
            transform.rotation = Quaternion.Euler(90, 0, 0);
            ResetFluid();
        }

        public void ResetFluid() {
            points = new List<PointMetadata>();
            xPoints = new SortedSet<float>();
            zPoints = new SortedSet<float>();
            minY = float.MaxValue;
            maxY = float.MinValue;
            cooldowns = new Dictionary<GameObject, float>();
        }

        private void FixedUpdate() { UpdateCooldowns(); }

        /*private void OnEnable() {
            Fluid = fluidAsset.GetFluid();
            for (int x = 0; x < 4; x++) {
                for (int z = 0; z < 4; z++) {
                    var pos = transform.position + new Vector3(x * 2, z * 2, x * 2);
                    AddPoint(pos);
                }
            }
        }*/

        private void Update() {
            UpdatePoints();
            boxCollider.size = Scale;
            boxCollider.center = Center;
            /*if (points is not null && points.Count > 0) */RenderPuddle();
            //Debug.Log("Rendering Puddle!!!!");
        }

        /*private void OnDrawGizmos() {
            if (points is null) return;
            foreach (PointMetadata metadata in points) {
                Vector3 pos = new Vector3(metadata.Point.Pos.x, 0f, metadata.Point.Pos.y);
                pos += transform.position;

                Gizmos.color = metadata.Point.PrimaryColor;
                Gizmos.DrawSphere(pos, metadata.Point.Size / 2f);
            }
        }*/

        private void OnTriggerStay(Collider other) {
            if (other.gameObject.layer == LayerMask.GetMask("Room")) return;

            cooldowns.TryAdd(other.gameObject, Fluid.Cooldown);

            if (!(
                Physics.Raycast(new Ray(other.transform.position, Vector3.down), out var hit)
                && cooldowns[other.gameObject] <= 0
            )) return;

            Vector2 pos = hit.point;
            foreach (var ptMetadata in points) {
                var pt = ptMetadata.Point;
                Vector2 dist = pt.Pos - pos;
                if (dist.sqrMagnitude <= pt.Size * pt.Size) {
                    fluid.ApplyEffect(
                        other.gameObject,
                        fluid.LifeProgress(ptMetadata.Lifetime, ptMetadata.SecondsActive)
                    );
                }
            }
        }

        private void UpdateCooldowns() {
            List<GameObject> toRemove = new List<GameObject>();

            foreach (var key in cooldowns.Keys.ToList()) {
                var newCooldown = cooldowns[key] -= Time.deltaTime;
                if (newCooldown <= 0) {
                    //cooldowns.Remove(key);
                    toRemove.Add(key);
                } else { cooldowns[key] = newCooldown; }
            }

            foreach (GameObject key in toRemove) cooldowns.Remove(key);
        }

        private void UpdatePoints() {
            if (points is null || points.Count <= 0) return;
            for (int i = points.Count - 1; i >= 0; i--) {
                points[i].Update(Fluid);
                if (!points[i].IsActive) {
                    var pos = points[i].Point.Pos;
                    xPoints.Remove(pos.x);
                    zPoints.Remove(pos.y);
                    points.RemoveAt(i);
                }
            }

            if (points.Count == 0) Destroy(gameObject);
        }

        public void AddPoint(Vector3 pos) {
            var pointMetadata = new PointMetadata(Fluid, new Vector2(pos.x, pos.z));
            points.Add(pointMetadata);
            xPoints.Add(pos.x);
            zPoints.Add(pos.z);
            minY = Mathf.Min(minY, pos.y);
            maxY = Mathf.Max(maxY, pos.y);
            boxCollider.size = Scale;
        }

        private void RenderPuddle() {
            pointsBuffer?.Release();
            if (points.Count <= 0) return;
            var pointsArray = points.Select(metaData => metaData.Point).ToArray();
            pointsBuffer = new ComputeBuffer(
                pointsArray.Length, Marshal.SizeOf<Point>(),
                ComputeBufferType.Structured
            );
            
            pointsBuffer.SetData(pointsArray);
            
            //Debug.Log(pointsBuffer.IsValid());
            var min = Min;
            var scale = Scale;

            projector.uvScale = new Vector2(scale.x, scale.z);
            projector.uvBias = new Vector2(min.x, min.z);

            puddleMaterial.SetInteger("PointCount", pointsArray.Length);
            puddleMaterial.SetBuffer("Points", pointsBuffer);
            //Graphics.DrawMesh(cubeMesh, localToWorld, puddleMaterial, 0);
        }

        private void OnDestroy() {
            CoreUtils.Destroy(cubeMesh);
            CoreUtils.Destroy(puddleMaterial);
        }
    }
}