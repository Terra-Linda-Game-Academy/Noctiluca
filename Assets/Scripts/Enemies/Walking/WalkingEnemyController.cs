using AI;
using Input.ConcreteInputProviders.Enemy;
using Input.Data.Enemy;
using UnityEngine;

namespace Enemies.Walking {
	[RequireComponent(typeof(Perceptron))]
	[RequireComponent(typeof(EnemyHealthController))]
	public class WalkingEnemyController : MonoBehaviour {
		public  WalkingEnemyInputProvider providerTemplate;
		private WalkingEnemyInputProvider _provider;

		private Perceptron _perceptron;
		
		private EnemyHealthController _healthController;

		public float speed;

		private void OnEnable() {
			_perceptron = GetComponent<Perceptron>();

			_healthController = GetComponent<EnemyHealthController>();

			_provider = (WalkingEnemyInputProvider) providerTemplate.Clone(_perceptron);
		}

		private void FixedUpdate() { HandleInput(_provider.GetInput()); }

		private void HandleInput(WalkingEnemyInput inputData) {
			transform.position += new Vector3(inputData.Movement.x, 0.0f, inputData.Movement.y) * speed;
			if (inputData.PlayerPos != Vector3.zero)
				transform.LookAt(new Vector3(inputData.PlayerPos.x, transform.position.y, inputData.PlayerPos.z));
		}
	}
}