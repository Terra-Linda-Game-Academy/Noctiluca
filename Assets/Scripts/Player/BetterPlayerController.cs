using AI;
using Input.ConcreteInputProviders;
using Input.Data;
using UnityEngine;
using Util;

namespace Player {
	[RequireComponent(typeof(Perceptron))]
	[RequireComponent(typeof(Rigidbody))]
	public class BetterPlayerController : MonoBehaviour {
		[SerializeField] private PlayerInputProvider       inputProvider;
		[SerializeField] private RuntimeVar<MonoBehaviour> playerVar;

		[SerializeField, Range(0f, 100f)] //How fast the player can change direction, how snappy the control is.
		float maxAcceleration = 10f, maxAirAcceleration = 1f; //acceleration should be more sluggish in the air.

		[SerializeField, Range(0f, 100f)] float maxSpeed   = 10f;
		[SerializeField, Range(0f, 10f)]  float jumpHeight = 2f;

		[SerializeField, Range(0f, 90f)] float
			maxGroundAngle =
				25f; // Threshold to determine if ground below player is a valid to walk on, See: OnValidate()

		[SerializeField, Range(0, 5)]             int   maxAirJumps          = 0;
		[SerializeField, Range(100, 1000)] public float maxRotationSpeed     = 500f, maxAirRotationSpeed;
		[SerializeField]                   public float minVelocityThreshold = 0.1f;

		private Rigidbody _body;
		private Vector3   _contactNormal;
		private Vector3   _velocity, _desiredVelocity;
		private int       _groundContactCount;
		private int       _jumpPhase;
		private bool      OnGround => _groundContactCount > 0;
		private bool      _desiredJump;
		private float     _minGroundDotProduct;

		private void OnEnable() { playerVar.Value = this; }

		private void OnDisable() { playerVar.Value = null; }

		private void Start() {
			//initialize some variables...
			_body = GetComponent<Rigidbody>();
			OnValidate();
			
			inputProvider.RequireInit(GetComponent<Perceptron>());
			inputProvider.Events.Interact += () => {
				                                 Debug.Log("interact");
				                                 Jump();
			                                 };
		}

		private void OnValidate() {
			_minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
			/* Checks how perpendicular the contact normal is to determine if the player is on the ground or not.
		 * By using the cosine value of the maximum angle allowed and the upward direction the `minGroundDotProduct`
		 * can be used as a threshold value to determine if the player is on a valid ground surface.
		 * OnValidate() is used to update the value when values are changed in the editor.
		*/
		}

		/*private void Update() {
			// Read the player's input from the horizontal and vertical axes
			Vector2 playerInput = new Vector2(0f, 0f);
			playerInput.x = UnityEngine.Input.GetAxis("Horizontal");
			playerInput.y = UnityEngine.Input.GetAxis("Vertical");

			// Limit the magnitude of the player's input vector to 1 to prevent diagonal movement from being faster
			playerInput = Vector2.ClampMagnitude(playerInput, 1f);

			// Convert the 2D player input into a 3D desired velocity vector
			_desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

			// Check if the player has pressed the jump button
			_desiredJump |= UnityEngine.Input.GetButtonDown("Jump");
		}*/

		private void FixedUpdate() {
			PlayerInput input = inputProvider.GetInput();

			_desiredVelocity = new Vector3(input.Movement.x, 0f, input.Movement.y) * maxSpeed;

			UpdateState();
			AdjustVelocity();
			RotatePlayer(_desiredVelocity);

			_body.velocity = _velocity;
			ClearState();
		}

		void UpdateState() {
			_velocity = _body.velocity;
			if (OnGround) {
				_jumpPhase = 0;
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

		void Jump() {
			if (OnGround || _jumpPhase < maxAirJumps) {
				_jumpPhase += 1;
				float jumpSpeed    = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
				float alignedSpeed = Vector3.Dot(_velocity, _contactNormal);
				if (alignedSpeed > 0f) { jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f); }

				_velocity += _contactNormal * jumpSpeed;
			}
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

		void RotatePlayer(Vector3 vector) //TODO: Change this method because its so bad
		{
			if (vector.magnitude > minVelocityThreshold) {
				Quaternion targetRotation = Quaternion.LookRotation(vector.normalized, Vector3.up);
				targetRotation.x = 0f;
				targetRotation.z = 0f;
				float rotationSpeed = OnGround ? maxRotationSpeed : maxAirRotationSpeed;
				transform.rotation =
					Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
			}
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