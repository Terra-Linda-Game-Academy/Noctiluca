using AI;
using Input.ConcreteInputProviders.Enemy;
using Input.Data.Enemy;
using UnityEngine;

namespace Enemies.Draco {
	[RequireComponent(typeof(Perceptron))]
	[RequireComponent(typeof(EnemyHealthController))]
	public class DracoController : MonoBehaviour {
		public  DracoInputProvider providerTemplate;
		private DracoInputProvider _provider;

		private Perceptron _perceptron;

		private EnemyHealthController _healthController;

		public float speed = 1.0f;

		public GameObject projectilePrefab;

		private Vector3 _playerPos;

		private void OnEnable() {
			_perceptron = GetComponent<Perceptron>();

			_healthController = GetComponent<EnemyHealthController>();

			_provider = (DracoInputProvider) providerTemplate.Clone(_perceptron);

			_provider.Events.Shoot += Shoot;

			_healthController.OnZero += () => { Destroy(gameObject); };
		}

		private void FixedUpdate() { HandleInput(_provider.GetInput()); }

		private void HandleInput(DracoInput inputData) {
			transform.position += inputData.Movement * speed;

			if (inputData.LookDir != Quaternion.identity) transform.rotation = inputData.LookDir;

			_playerPos = inputData.PlayerPos;
		}

		private void Shoot() {
			Vector3 offset = transform.forward;

			GameObject obj = Instantiate(projectilePrefab, transform.position + offset, Quaternion.identity);
			obj.transform.LookAt(_playerPos);
		}
	}
}