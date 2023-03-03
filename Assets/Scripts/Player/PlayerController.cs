using AI;
using Input.ConcreteInputProviders;
using Input.Data;
using UnityEngine;
using Util;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// Author : EvanisEpicandCool
        ///                                                 
        /// Simple Character/Player Controller to take in external inputmap modifiers
        /// also has some Movement code that is on a string
        /// 
        /// Date : 11/9/22
        /// </summary>

        //Just a Couple Movement paramaters
        [SerializeField] public float movementSpeed { get; private set; } = 3;

        [SerializeField] private float rotationSpeed = 10;
        [SerializeField] private PlayerInputProvider inputProvider;
        [SerializeField] private RuntimeVar<MonoBehaviour> playerVar;

        private Perceptron _perceptron;

		private PlayerHealthController _healthController;

		//Movement Vectors and Bools
		private Vector3    _direction;
		private Quaternion Rotation => Quaternion.LookRotation(RotationDirection);

        private Vector3 RotationDirection =>
            Vector3.RotateTowards(transform.forward, _direction, rotationSpeed * Time.deltaTime, 0);

        private void OnEnable() { playerVar.Value = this; }

        private void OnDisable() { playerVar.Value = null; }

		private void Start() {
			_perceptron      = GetComponent<Perceptron>();

			_healthController = GetComponent<PlayerHealthController>();

			inputProvider.RequireInit(_perceptron);
			inputProvider.Events.Interact += () => Debug.Log("player interact");
		}

        private void FixedUpdate()
        {
            PlayerInput input = inputProvider.GetInput();

            _direction = new Vector3(input.Movement.x, 0, input.Movement.y);

            transform.position += _direction * (movementSpeed * Time.deltaTime);
            transform.rotation = Rotation;
        }
    }
}
