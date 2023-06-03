using System;
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
			if (other.gameObject.layer != LayerMask.NameToLayer("Room")) return;

			GameObject puddleObj = new GameObject {transform = {position = transform.position}, name = "Puddle"};

			Puddle puddle = puddleObj.AddComponent<Puddle>();
			puddle.Fluid = _potion.Fluid;

			float radius      = _potion.Remaining * _potion.Fluid.Size;
			int   numOfPoints = (int) (_potion.Remaining * 10f);

			for (int i = 0; i < numOfPoints; i++) {
				Vector2 point = Random.insideUnitCircle * radius;

				puddle.AddPoint(new Vector3(point.x, transform.position.y, point.y));
			}

			Destroy(gameObject);
		}
	}
}