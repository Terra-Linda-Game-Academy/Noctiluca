using System.Collections;
using AI;
using Input.ConcreteInputProviders;
using Input.Data;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Util;
using System.Collections;
using UnityEngine.Serialization;

namespace Player {
	[RequireComponent(typeof(Perceptron))]
	[RequireComponent(typeof(Rigidbody))]
	public class BetterPlayerController : MonoBehaviour {
		[SerializeField] private PlayerInputProvider       inputProvider;
		[SerializeField] private RuntimeVar<MonoBehaviour> playerVar;

		[SerializeField] private Transform stepCheck;
		[SerializeField] private Transform headCheck;

		[SerializeField, Range(0f, 100f)] //How fast the player can change direction, how snappy the control is.
		float maxAcceleration = 10f, maxAirAcceleration = 1f; //acceleration should be more sluggish in the air.

		[SerializeField] float jumpheight;
		[SerializeField] float stepheight;

		[SerializeField, Range(0f, 100f)]          float    maxSpeed = 10f;
		[FormerlySerializedAs("_animator")] public Animator animator;

		[SerializeField, Range(0f, 90f)] float
			maxGroundAngle =
				25f; // Threshold to determine if ground below player is a valid to walk on, See: OnValidate()

		[SerializeField, Range(100, 9000)] public float maxRotationSpeed     = 500f, maxAirRotationSpeed;
		[SerializeField]                   public float minVelocityThreshold = 0.1f;

		private Rigidbody _body;
		private Vector3   _contactNormal;
		private Vector3   _velocity, _desiredVelocity;
		private int       _groundContactCount;
		private bool      OnGround => _groundContactCount > 0;
		private bool      _desiredJump;
		private float     _minGroundDotProduct;
		Animator          m_Animator;

		private SwordAttack _attack;

		private static readonly int Saunter  = Animator.StringToHash("Saunter");
		private static readonly int Idle     = Animator.StringToHash("Idle");
		private static readonly int Attack   = Animator.StringToHash("Attack");
		private static readonly int Throwing = Animator.StringToHash("Throwing");

		private void OnEnable() { playerVar.Value = this; }

		private void OnDisable() { playerVar.Value = null; }

		IEnumerator attack() {
			animator.SetTrigger(Attack);

			Debug.Log("attacdking");
			new WaitForSeconds(.5f);

			animator.SetTrigger(Idle);

			yield return null;
		}

		IEnumerator throwtion() {
			animator.SetTrigger(Throwing);
			Debug.Log("throwing");
			new WaitForSeconds(3);

			animator.SetTrigger(Idle);

			yield return null;
		}

		private void Start() {
			//initialize some variables...
			_body = GetComponent<Rigidbody>();
			OnValidate();

			_attack = GetComponent<SwordAttack>();

			inputProvider.RequireInit(GetComponent<Perceptron>());
			inputProvider.Events.Interact += () => { Debug.Log("interact"); };
			//inputProvider.Events.Attack   += _attack.Attack;
			inputProvider.Events.Attack += () => { StartCoroutine(nameof(attack)); };

			inputProvider.Events.Throw += () => { StartCoroutine(nameof(throwtion)); };
		}


		private void OnValidate() {
			_minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
			/* Checks how perpendicular the contact normal is to determine if the player is on the ground or not.
		 * By using the cosine value of the maximum angle allowed and the upward direction the `minGroundDotProduct`
		 * can be used as a threshold value to determine if the player is on a valid ground surface.
		 * OnValidate() is used to update the value when values are changed in the editor.
		*/
		}

		private void FixedUpdate() {
			if (Physics.Raycast(stepCheck.position, stepCheck.TransformDirection(Vector3.forward), .6f)
			 && !Physics.Raycast(headCheck.position, headCheck.TransformDirection(Vector3.forward), .6f)) {
				Debug.Log("blame jackson for the jank, not me");
				transform.position = new Vector3(transform.position.x + transform.forward.x * 1.005f,
				                                 transform.position.y + .1f,
				                                 transform.position.z + transform.forward.z * 1.005f);
			}

			PlayerInput input = inputProvider.GetInput();

			_desiredVelocity  = new Vector3(input.Movement.x, 0f, input.Movement.y) * maxSpeed;
			_attack.attackDir = new Vector3(input.Aim.x, 0f, input.Aim.y);

			UpdateState();
			AdjustVelocity();
			RotatePlayer(_desiredVelocity);
			/*if(_desiredVelocity.magnitude > 0)
			{
				//start moving anim
				m_Animator.SetTrigger("JogStart");
			} else {
				//stop moving anim
			}
			*/
			_body.velocity = _velocity;
			UpdateAnimation();
			ClearState();
		}

		void UpdateAnimation() {
			if (_body.velocity != Vector3.zero
			 && !animator.GetCurrentAnimatorStateInfo(0).IsName("Throwing")
			 && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
				animator.SetTrigger(Saunter);
				animator.ResetTrigger(Idle);
				/*Debug.Log("sauntering");*/
			} else {
				animator.ResetTrigger(Saunter);
				animator.SetTrigger(Idle); /*Debug.Log("idling");*/
			}

			//if (true)
			//{
			//🍿
			//
		}

		void UpdateState() {
			_velocity = _body.velocity;
			if (OnGround) {
				if (_groundContactCount > 1) { _contactNormal.Normalize(); }
			} else { _contactNormal = Vector3.up; }
		}

		// This method adjusts the velocity of the character based on input and the current state.
		void AdjustVelocity() {
			//Projects the x and z axis to line up with the ground contact plane so that movement follows the slope of the ground.
			Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
			Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

			/*Calculates the current velocity of the player in the x and z directions by taking the dot product of the current
		 velocity as well as the ground-aligned x and z axes. I really wish they taught linear algebra in school. */
			float currentX = Vector3.Dot(_velocity, xAxis);
			float currentZ = Vector3.Dot(_velocity, zAxis);

			//Calculates the maximum acceleration that can occur in one frame based on whether the player is grounded or not.
			float acceleration   = OnGround ? maxAcceleration : maxAirAcceleration;
			float maxSpeedChange = acceleration * Time.deltaTime;

			//Calculates the new velocities by calling math.MoveTowards on the current and desired velocities.
			float newX = Mathf.MoveTowards(currentX, _desiredVelocity.x, maxSpeedChange);
			float newZ = Mathf.MoveTowards(currentZ, _desiredVelocity.z, maxSpeedChange);

			//Finally applies the new values to the x and z axis to create the new velocity of the player. 
			_velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
		}

		void ClearState() {
			_groundContactCount = 0;
			_contactNormal      = Vector3.zero;
		}

		Vector3 ProjectOnContactPlane(Vector3 vector) {
			return vector - _contactNormal * Vector3.Dot(vector, _contactNormal);
			/* Project planes along contact normal. E. g. player movement x axis is projected along contact normal so
		 * that movement follows the slope of the ground instead of clipping into it.
		 */
		}

		//TODO: Change this method because its so bad
		// it is pretty funny tho - jackson r
		void RotatePlayer(Vector3 vector) {
			if (!(vector.magnitude > minVelocityThreshold)) return;
			Quaternion targetRotation = Quaternion.LookRotation(vector.normalized, Vector3.up);
			targetRotation.x = 0f;
			targetRotation.z = 0f;
			float rotationSpeed = OnGround ? maxRotationSpeed : maxAirRotationSpeed;
			transform.rotation =
				Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
		}


		void OnCollisionEnter(Collision collision) { EvaluateCollision(collision); }

		void OnCollisionStay(Collision collision) { EvaluateCollision(collision); }

		void EvaluateCollision(Collision collision) {
			/* I forgot what this method does. Looks like it might be a traversal to add up the contact point normals
		 * that are not overly steep to create the contactNormal but your guess is probably better than mine.
		 */
			for (int i = 0; i < collision.contactCount; i++) {
				Vector3 normal = collision.GetContact(i).normal;
				if (normal.y >= _minGroundDotProduct) {
					_groundContactCount += 1;
					_contactNormal      += normal;
				}
			}
		}
	}
}