using AI;
using Input.ConcreteInputProviders.Enemy;
using Input.Data.Enemy;
using Snake;
using UnityEngine;

namespace Enemies.Snake
{
	[RequireComponent(typeof(Perceptron))]
	public class FireSnakeController : MonoBehaviour
	{
		public SlitherNavigationTest slitherNavigationTest;

		public FireSnakeInputProvider providerTemplate;
		private FireSnakeInputProvider _provider;

		private Perceptron _perceptron;

		public float speed;

		private void OnEnable()
		{
			_perceptron = GetComponent<Perceptron>();

			_provider = (FireSnakeInputProvider)providerTemplate.Clone(_perceptron);
		}

		private void FixedUpdate() { HandleInput(_provider.GetInput()); }

		private void HandleInput(FireSnakeInput inputData)
		{
			slitherNavigationTest.targetDestination = inputData.TargetDestination;
			//transform.position += new Vector3(inputData.Movement.x, 0.0f, inputData.Movement.y) * speed;
			//if (inputData.PlayerPos != Vector3.zero)
				//transform.LookAt(new Vector3(inputData.PlayerPos.x, transform.position.y, inputData.PlayerPos.z));
		}
	}
}