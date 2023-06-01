using System;
using System.Collections;
using Potions.Fluids;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Potions {
	public class ThrownPotionController : MonoBehaviour {
		private Potion _potion;

		private MeshRenderer _meshRenderer;

		private static readonly int Color = Shader.PropertyToID("_Color");

		public void Init(Potion potion) {
			_potion = potion;

			_meshRenderer = GetComponent<MeshRenderer>();

			_meshRenderer.material.SetColor(Color, _potion.Fluid.PrimaryColor.Evaluate(0));
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