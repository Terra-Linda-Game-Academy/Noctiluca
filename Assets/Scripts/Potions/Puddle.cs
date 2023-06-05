using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Potions.Fluids;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Potions {
	[RequireComponent(typeof(BoxCollider))]
	//[ExecuteInEditMode]
	public class Puddle : MonoBehaviour {
		private class PointMetadata {
			public readonly  float Lifetime;
			private readonly float initialSize;
			private          float secondsActive;
			public           float SecondsActive => secondsActive;
			public           Point Point         { get; private set; }

			public bool IsActive { get; private set; }


			public PointMetadata(Fluid fluid, Vector3 position) {
				Lifetime    = fluid.InitialLifetime;
				initialSize = fluid.InitialSize;
				Point = new Point(
					fluid.PrimaryColor.Evaluate(0),
					fluid.SecondaryColor.Evaluate(0),
					position,
					initialSize
				);
				secondsActive = 0;
				IsActive      = true;
			}

			public void Update(Fluid fluid) {
				secondsActive += Time.deltaTime;
				var lifetimeProgress = fluid.LifeProgress(Lifetime, secondsActive);

				Color primary   = fluid.PrimaryColor.Evaluate(lifetimeProgress);
				Color secondary = fluid.SecondaryColor.Evaluate(lifetimeProgress);

				Point = new Point(primary, secondary, Point.Pos, initialSize * (1 - lifetimeProgress));

				if (secondsActive >= Lifetime) { IsActive = false; }
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct Point {
			private float size;
			public  float Size => size;

			private readonly ushort x;
			private readonly ushort y;
			private readonly ushort z;
			public Vector3 Pos => new Vector3(Mathf.HalfToFloat(x), Mathf.HalfToFloat(y), Mathf.HalfToFloat(z));

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

			public Point(Color primary, Color secondary, Vector3 position, float size) {
				x         = Mathf.FloatToHalf(position.x);
				y         = Mathf.FloatToHalf(position.y);
				z         = Mathf.FloatToHalf(position.z);
				this.size = size;

				r1 = g1 = b1 = 0;
				r2 = g2 = b2 = 0;

				PrimaryColor   = primary;
				SecondaryColor = secondary;
			}
		}

		private Mesh        cylinderMesh;
		private Material    puddleMaterial;
		private BoxCollider boxCollider;
		private Fluid       fluid;

		public Fluid Fluid {
			get => fluid;
			set {
				if (value == fluid) return;
				fluid = value;
				ResetFluid();
			}
		}

		private List<PointMetadata> points;
		private SortedSet<float>    xPoints;
		private SortedSet<float>    zPoints;
		private float               minY;
		private float               maxY;

		private Dictionary<GameObject, float> cooldowns;

		private Vector3 Scale =>
			new Vector3(xPoints.Max - xPoints.Min, Mathf.Max(maxY - minY, 1), zPoints.Max - zPoints.Min);

		private Vector3 Min => new Vector3(xPoints.Min, minY, zPoints.Min);

		private Vector3 Center =>
			new Vector3(
				(xPoints.Min + xPoints.Max) / 2,
				(minY        + maxY)        / 2,
				(xPoints.Min + zPoints.Max) / 2
			);

		private void Awake() {
			cylinderMesh                    = Resources.GetBuiltinResource<Mesh>("New-Cylinder.fbx");
			puddleMaterial                  = Resources.Load<Material>("PuddleMaterial");
			puddleMaterial.enableInstancing = true;
			boxCollider                     = GetComponent<BoxCollider>();
			boxCollider.isTrigger           = true;
			transform.rotation              = Quaternion.Euler(90, 0, 0);
			gameObject.layer                = LayerMask.NameToLayer("Ignore Raycast");
			ResetFluid();
		}

		public void ResetFluid() {
			points    = new List<PointMetadata>();
			xPoints   = new SortedSet<float>();
			zPoints   = new SortedSet<float>();
			minY      = float.MaxValue;
			maxY      = float.MinValue;
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
			var scale = Scale;
			boxCollider.size = new Vector3(scale.x, scale.z, scale.y);
			//boxCollider.center = Center;
			/*if (points is not null && points.Count > 0) */
			RenderPuddle();
			//Debug.Log("Rendering Puddle!!!!");
		}

		private void OnDrawGizmos() {
			if (points is null) return;
			Gizmos.matrix = Matrix4x4.identity;
			foreach (PointMetadata metadata in points) {
				Vector3 pos = new Vector3(metadata.Point.Pos.x, 0f, metadata.Point.Pos.y);
				//pos += transform.position;

				Gizmos.color = metadata.Point.PrimaryColor;
				Gizmos.DrawSphere(pos, metadata.Point.Size);
			}
		}

		private void OnTriggerStay(Collider other) {
			if (other.gameObject.layer == LayerMask.GetMask("Room")) return;

			/*if (!(
			    Physics.Raycast(
			        new Ray(other.transform.position, Vector3.down), 
			        out var hit, 0.1f, ~LayerMask.NameToLayer("Room"))
			    && cooldowns[other.gameObject] <= 0
			)) return;*/

			if (!Physics.Raycast(new Ray(other.transform.position, Vector3.down), out var hit, 5f,
			                     ~LayerMask.GetMask("Room"))) { return; }

			if (cooldowns.ContainsKey(other.gameObject) && cooldowns[other.gameObject] > 0) return;

			Vector2 pos = new Vector2(hit.point.x, hit.point.z);
			foreach (var ptMetadata in points) {
				var     pt   = ptMetadata.Point;
				Vector2 dist = new Vector2(pt.Pos.x, pt.Pos.z) - pos;
				if (dist.sqrMagnitude <= pt.Size * pt.Size) {
					fluid.ApplyEffect(
						other.gameObject,
						fluid.LifeProgress(ptMetadata.Lifetime, ptMetadata.SecondsActive)
					);
					break;
				}
			}

			cooldowns.TryAdd(other.gameObject, Fluid.Cooldown);
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
			var pointMetadata = new PointMetadata(Fluid, pos);
			points.Add(pointMetadata);
			xPoints.Add(pos.x);
			zPoints.Add(pos.z);
			minY             = Mathf.Min(minY, pos.y);
			maxY             = Mathf.Max(maxY, pos.y);
			boxCollider.size = Scale;
		}

		private void RenderPuddle() {
			if (points.Count <= 0) return;
			MaterialPropertyBlock properties = new MaterialPropertyBlock();

			Matrix4x4[] matrices = new Matrix4x4[points.Count];
			Vector4[]   colors   = new Vector4[points.Count];
			var         pos      = transform.position;

			for (int i = 0; i < points.Count; i++) {
				var pt = points[i].Point;
				matrices[i] = Matrix4x4.TRS(
					pt.Pos,
					Quaternion.identity,
					new Vector3(pt.Size, 0.1f, pt.Size)
				);
				var color = pt.PrimaryColor;
				colors[i] = new Vector4(color.r, color.g, color.b, 1);
			}

			properties.SetVectorArray("_BaseColor", colors);

			Graphics.DrawMeshInstanced(
				cylinderMesh, 0, puddleMaterial,
				matrices, points.Count, properties, ShadowCastingMode.Off
			);


			//Graphics.DrawMeshInstanced(cylinderMesh, 0, puddleMaterial, );
			/*pointsBuffer?.Release();
			if (points.Count <= 0) return;
			var pointsArray = points.Select(metaData => metaData.Point).ToArray();
			pointsBuffer = new ComputeBuffer(
			    pointsArray.Length, Marshal.SizeOf<Point>(),
			    ComputeBufferType.Structured
			);
			
			pointsBuffer.SetData(pointsArray);

			transform.position = Center;
			
			//Debug.Log(pointsBuffer.IsValid());
			var min = Min;
			var scale = Scale;

			projector.size = new Vector3(scale.x, scale.z, scale.y);
			projector.uvScale = new Vector2(scale.x, scale.z);
			projector.uvBias = new Vector2(min.x, min.z);

			puddleMaterial.SetInteger("PointCount", pointsArray.Length);
			puddleMaterial.SetBuffer("Points", pointsBuffer);
			
			
			//Graphics.DrawMesh(cubeMesh, localToWorld, puddleMaterial, 0);*/
		}
	}
}