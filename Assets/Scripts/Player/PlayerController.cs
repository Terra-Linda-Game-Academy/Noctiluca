using System;
using Input;
using Input.ConcreteInputProviders;
using UnityEngine;

namespace Player {
	public class PlayerController : MonoBehaviour {
		/// <summary>
		/// Author : EvanisEpicandCool
		/// 
		/// Description : And I will cast abominable filth upon thee,
		///                         and make thee vile,
		///                and will set thee as a gazingstock.
		///                                                 -Nahum 3:6
		///                                                 
		/// Simple Character/Player Controller to take in external inputmap modifiers
		/// also has some Movement code that is on a string
		/// 
		/// Date : 11/9/22
		/// 
		/// </summary>

		//Just a Couple Movement paramaters
		[SerializeField] private float movementSpeed = 3;

		[SerializeField] private float rotationSpeed = 10;

		[SerializeField] private PlayerInputProvider inputProvider;

		//Movement Vectors and Bools
		private Vector3    _direction;
		private Quaternion Rotation => Quaternion.LookRotation(RotationDirection);

		private Vector3 RotationDirection =>
			Vector3.RotateTowards(transform.forward, _direction, rotationSpeed * Time.deltaTime, 0);

		private void Start() {
			inputProvider.RequireInit();
			inputProvider.Events.OnInteract += () => { Debug.Log("interact club headed by randall baker"); };
		}

		private void FixedUpdate() {
			PlayerInputData inputData = inputProvider.GetInputData();

			_direction = new Vector3(inputData.movement.x, 0, inputData.movement.y);

			transform.position += _direction * (movementSpeed * Time.deltaTime);
			transform.rotation =  Rotation;
		}
	}
}