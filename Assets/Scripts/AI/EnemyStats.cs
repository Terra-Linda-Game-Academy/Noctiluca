using UnityEngine;

namespace AI {
	[CreateAssetMenu]
	public class EnemyStats : ScriptableObject {
		/// <summary>
		/// The radius of the looking spherecast.
		/// </summary>
		public float lookRadius = 1.0f;
		
		/// <summary>
		/// The maximum visibility range;
		/// </summary>
		public float lookRange  = 20.0f;
	}
}