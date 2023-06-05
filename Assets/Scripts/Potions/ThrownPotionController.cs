using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Potions {
	public class ThrownPotionController : MonoBehaviour {
		private Potion _potion;

		private MeshRenderer _meshRenderer;

		private static readonly int Color = Shader.PropertyToID("_Color");

		private const float MaxLifetime = 5f;
		private       float _startTime;

		public void Init(Potion potion) {
			_potion = potion;

			_meshRenderer = GetComponent<MeshRenderer>();

			_meshRenderer.material.SetColor(Color, _potion.Fluid.PrimaryColor.Evaluate(0));

			_startTime = Time.time;
		}

		private void Update() {
			if (Time.time - _startTime > MaxLifetime) Destroy(gameObject);
		}

		private void OnCollisionEnter(Collision other) {
			if (other.gameObject.layer != LayerMask.NameToLayer("Room")
			 && other.gameObject.layer != LayerMask.NameToLayer("Enemy"))
				return;

			var pos = transform.position;
			GameObject puddleObj = new GameObject {transform = {position = pos}, name = "Puddle"};

			Puddle puddle = puddleObj.AddComponent<Puddle>();
			puddle.Fluid = _potion.Fluid;

			float radius      = _potion.Fluid.Size * 4;
			int numOfPoints = 25;

			for (int i = 0; i < numOfPoints; i++) {
				Vector2 point = new Vector2(pos.x, pos.z) + Random.insideUnitCircle * radius;

				Vector3 pt3d = new Vector3(point.x, pos.y + 0.1f, point.y);

				if (Physics.Raycast(
					new Ray(pt3d, Vector3.down),
					out var hit, 2, ~LayerMask.NameToLayer("Room")
				)) {
					puddle.AddPoint(pt3d);
				}
			}

			Destroy(gameObject);
		}
	}
}