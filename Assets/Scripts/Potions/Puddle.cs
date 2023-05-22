using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Potions.Fluids;
using UnityEngine;
using UnityEngine.Rendering;

namespace Potions {
    [RequireComponent(typeof(BoxCollider))]
    public class Puddle : MonoBehaviour { 
        private struct PointMetadata {
            private readonly float lifetime;
            private readonly float initialSize;
            private float secondsActive;
            public Point Point { get; private set; }
            public bool IsActive { get; private set; }

            public PointMetadata(Fluid fluid, Vector2 position) {
                lifetime = fluid.InitialLifetime;
                initialSize = fluid.InitialSize;
                Point = new Point(fluid, position, initialSize);
                secondsActive = 0;
                IsActive = true;
            }

            public void Update(Fluid fluid) {
                secondsActive += Time.deltaTime;
                var lifetimeProgress = fluid.LifeProgress(lifetime, secondsActive);
                Point.Update(fluid, lifetimeProgress, initialSize);
                if (secondsActive >= lifetime) { IsActive = false; }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Point {
            private float size;
            
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

        private Vector3 Scale => new Vector3(xPoints.Max - xPoints.Min, maxY - minY, zPoints.Max - zPoints.Min);
        private Vector3 Min => new Vector3(xPoints.Min, minY, zPoints.Min);
        private Vector3 Center => 
            new Vector3(
                (xPoints.Min + xPoints.Max) / 2,
                (minY + maxY) / 2, 
                (xPoints.Min + zPoints.Max) / 2
            );

        private void Awake() {
            cubeMesh = CoreUtils.CreateCubeMesh(Vector3.zero, Vector3.one);
            puddleMaterial = CoreUtils.CreateEngineMaterial("Puddle");
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
            for (int x = 0; x < 100; x++) {
                for (int z = 0; z < 100; z++) {
                    var pos = transform.position + new Vector3(x * .1f, z * .1f, x * .1f);
                    AddPoint(pos);
                }
            }
        }

        private void LateUpdate() {
            UpdatePoints();
            RenderPuddle();
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
            int i = 0;
            while (i < points.Count) {
                points[i].Update(Fluid);
                if (!points[i].IsActive) {
                    points.RemoveAt(i);
                    var pos = points[i].Point.Pos;
                    xPoints.Remove(pos.x);
                    zPoints.Remove(pos.y);
                } else {
                    i++;
                }
            }
        }

        public void AddPoint(Vector3 pos) {
            var pointMetadata = new PointMetadata(Fluid, new Vector2(pos.x, pos.y));
            points.Add(pointMetadata);
            xPoints.Add(pos.x);
            minY = Mathf.Min(minY, pos.z);
            maxY = Mathf.Max(maxY, pos.z);
            boxCollider.size = Scale;
            boxCollider.center = Center;
        }

        private void RenderPuddle() {
            var pointsBuffer = new ComputeBuffer(
                points.Count, Marshal.SizeOf<Point>(),
                ComputeBufferType.Structured, ComputeBufferMode.SubUpdates
            );
            var pointsData = pointsBuffer.BeginWrite<Point>(0, points.Count);
            for (int i = 0; i <= points.Count; i++) pointsData[i] = points[i].Point;
            pointsBuffer.EndWrite<Point>(points.Count);

            var min = Min; 
            var scale = Scale;
            var localToWorld = Matrix4x4.TRS(Min, Quaternion.identity, Scale);
            
            puddleMaterial.SetBuffer("Points", pointsBuffer);
            puddleMaterial.SetInteger("PointCount", points.Count);
            puddleMaterial.SetVector("ScaleOffset", new Vector4(scale.x, scale.z, min.x, min.z));

            Graphics.DrawMesh(cubeMesh, localToWorld, puddleMaterial, 0);

            pointsBuffer.Dispose();
        }

        private void OnDestroy() {
            CoreUtils.Destroy(cubeMesh);
            CoreUtils.Destroy(puddleMaterial);
        }
    }
}