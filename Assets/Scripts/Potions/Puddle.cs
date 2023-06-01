using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Potions.Fluids;
using UnityEngine;
using UnityEngine.Rendering;

namespace Potions {
    [RequireComponent(typeof(BoxCollider))]
    [ExecuteInEditMode]
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
                Point = new Point(fluid, position, initialSize);
                secondsActive = 0;
                IsActive = true;
            }

            public void Update(Fluid fluid) {
                secondsActive += Time.deltaTime;
                var lifetimeProgress = fluid.LifeProgress(Lifetime, secondsActive);
                Point.Update(fluid, lifetimeProgress, initialSize);
                if (secondsActive >= Lifetime) { IsActive = false; }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Point {
            private float size;
            public float Size => size;
            
            private readonly ushort x;
            private readonly ushort z;
            public Vector2 Pos => new Vector2(Mathf.HalfToFloat(x), Mathf.FloatToHalf(z));

            private ushort r1;
            private ushort g1;
            private ushort b1;
            private Color PrimaryColor {
                set {
                    r1 = Mathf.FloatToHalf(value.r);
                    g1 = Mathf.FloatToHalf(value.g);
                    b1 = Mathf.FloatToHalf(value.b);
                }
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

            public Point(Fluid fluid, Vector2 position, float initialSize) {
                x = Mathf.FloatToHalf(position.x);
                z = Mathf.FloatToHalf(position.y);
                size = initialSize;

                r1 = g1 = b1 = 0;
                r2 = g2 = b2 = 0;

                PrimaryColor = fluid.PrimaryColor.Evaluate(0);
                SecondaryColor = fluid.SecondaryColor.Evaluate(0);
            }

            public void Update(Fluid fluid, float lifetimeProgress, float initialSize) {
                size = initialSize * (1 - lifetimeProgress);
                PrimaryColor = fluid.PrimaryColor.Evaluate(lifetimeProgress);
                SecondaryColor = fluid.PrimaryColor.Evaluate(lifetimeProgress);
            }
        }

        private Mesh cubeMesh;
        private Material puddleMaterial;
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
        [SerializeField] private FluidAsset fluidAsset;

        private Vector3 Scale => new Vector3(xPoints.Max - xPoints.Min, Mathf.Max(maxY - minY, 1), zPoints.Max - zPoints.Min);
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

        private void FixedUpdate() {
            UpdateCooldowns();
        }

        private void OnEnable() {
            Fluid = fluidAsset.GetFluid();
            for (int x = 0; x < 4; x++) {
                for (int z = 0; z < 4; z++) {
                    var pos = transform.position + new Vector3(x * 2, z * 2, x * 2);
                    AddPoint(pos);
                }
            }
        }

        private void LateUpdate() {
            UpdatePoints();
            if (points.Count > 0)
                RenderPuddle();
            //Debug.Log("Rendering Puddle!!!!");
        }

        private void OnTriggerStay(Collider other) {
            if (!(
                    Physics.Raycast(new Ray(other.transform.position, Vector3.down), out var hit) && 
                    cooldowns[other.gameObject] <= 0
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
            foreach (var key in cooldowns.Keys) {
                var newCooldown = cooldowns[key] -= Time.deltaTime;
                if (newCooldown <= 0) {
                    cooldowns.Remove(key);
                } else {
                    cooldowns[key] = newCooldown;
                }
            }
        }

        private void UpdatePoints() {
            if (points.Count <= 0) return;
            for (int i = points.Count - 1; i >= 0; i--) {
                points[i].Update(Fluid);
                if (!points[i].IsActive) {
                    var pos = points[i].Point.Pos;
                    xPoints.Remove(pos.x);
                    zPoints.Remove(pos.y);
                    points.RemoveAt(i);
                }
            }
        }

        public void AddPoint(Vector3 pos) {
            var pointMetadata = new PointMetadata(Fluid, new Vector2(pos.x, pos.y));
            points.Add(pointMetadata);
            xPoints.Add(pos.x);
            zPoints.Add(pos.z);
            minY = Mathf.Min(minY, pos.y);
            maxY = Mathf.Max(maxY, pos.y);
            boxCollider.size = Scale;
            boxCollider.center = Center;
        }

        private void RenderPuddle() {
            var pointsArray = points.Select(metaData => metaData.Point).ToArray();
            var pointsBuffer = new ComputeBuffer(
                pointsArray.Length, Marshal.SizeOf<Point>(), 
                ComputeBufferType.Structured
            );
            pointsBuffer.SetData(pointsArray);

            var min = Min; 
            var scale = Scale;
            var localToWorld = Matrix4x4.TRS(min, Quaternion.identity, scale);
            
            puddleMaterial.SetBuffer("Points", pointsBuffer);
            puddleMaterial.SetInteger("PointCount", points.Count);
            puddleMaterial.SetVector("ScaleOffset", new Vector4(scale.x, scale.z, min.x, min.z));

            Graphics.DrawMesh(cubeMesh, localToWorld, puddleMaterial, 0);

            //pointsBuffer.Release();
        }

        private void OnDestroy() {
            CoreUtils.Destroy(cubeMesh);
            CoreUtils.Destroy(puddleMaterial);
        }
    }
}