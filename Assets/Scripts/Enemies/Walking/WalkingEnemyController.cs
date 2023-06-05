using AI;
using Input.ConcreteInputProviders.Enemy;
using Input.Data.Enemy;
using Player;
using UnityEngine;
using Util;

namespace Enemies.Walking {
	[RequireComponent(typeof(Perceptron))]
	[RequireComponent(typeof(EnemyHealthController))]
	public class WalkingEnemyController : MonoBehaviour {
		public  WalkingEnemyInputProvider providerTemplate;
		private WalkingEnemyInputProvider _provider;

		public ScriptableVar<MonoBehaviour> player;

		private Perceptron _perceptron;
		
		private EnemyHealthController _healthController;

		public float speed;

		private void OnEnable() {
			_perceptron = GetComponent<Perceptron>();

			_healthController = GetComponent<EnemyHealthController>();

			_healthController.OnZero += () => Destroy(gameObject);

			_provider = (WalkingEnemyInputProvider) providerTemplate.Clone(_perceptron);

			_provider.Events.Attack += Attack;
		}

		private void FixedUpdate() { HandleInput(_provider.GetInput()); }

		private void HandleInput(WalkingEnemyInput inputData) {
			transform.position += new Vector3(inputData.Movement.x, 0.0f, inputData.Movement.y) * speed;
			if (inputData.PlayerPos != Vector3.zero)
				transform.LookAt(new Vector3(inputData.PlayerPos.x, transform.position.y, inputData.PlayerPos.z));
		}

		private void Attack() {
			Debug.Log("ant attack");
			player.Value.GetComponent<PlayerHealthController>().Damage(1);
		}
	}
}