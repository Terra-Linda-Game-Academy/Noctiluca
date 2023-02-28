using System;
using AI;
using Input;
using Input.ConcreteInputProviders;
using Input.Data;
using UnityEngine;
using Util;

namespace Player {
	public class PlayerController : MonoBehaviour {
		/// <summary>
		/// Author : EvanisEpicandCool
		///                                                 
		/// Simple Character/Player Controller to take in external inputmap modifiers
		/// also has some Movement code that is on a string
		/// 
		/// Date : 11/9/22
		/// </summary>

		//Just a Couple Movement paramaters
		[SerializeField] private float movementSpeed = 3;

		[SerializeField] private float                     rotationSpeed = 10;
		[SerializeField] private PlayerInputProvider       inputProvider;
		[SerializeField] private RuntimeVar<MonoBehaviour> playerVar;

		private Perceptron _perceptron;

		//Movement Vectors and Bools
		private Vector3    _direction;
		private Quaternion Rotation => Quaternion.LookRotation(RotationDirection);

		private Vector3 RotationDirection =>
			Vector3.RotateTowards(transform.forward, _direction, rotationSpeed * Time.deltaTime, 0);

		private void OnEnable() { playerVar.Value = this; }

		private void OnDisable() { playerVar.Value = null; }

		private void Start() {
			_perceptron      = GetComponent<Perceptron>();
			_perceptron.eyes = transform;

			inputProvider.RequireInit(_perceptron);
			inputProvider.Events.Interact += () => Debug.Log("interact club headed by randall baker");
		}

		private void FixedUpdate() {
			PlayerInput input = inputProvider.GetInput();

			var camera = UnityEngine.Camera.main;

			//camera forward and right vectors:
			var forward = camera.transform.forward;
			var right = camera.transform.right;

			//project forward and right vectors on the horizontal plane (y = 0)
			forward.y = 0f;
			right.y = 0f;
			forward.Normalize();
			right.Normalize();

			Vector3 forwardRelative = input.Movement.x * forward;
			Vector3 rightRelative = input.Movement.y * right;

			Vector3 moveDir = forwardRelative + rightRelative;

			_direction = new Vector3(moveDir.z, 0, moveDir.x);

			transform.position += _direction * (movementSpeed * Time.deltaTime);
			transform.rotation =  Rotation;
		}
	}
}
