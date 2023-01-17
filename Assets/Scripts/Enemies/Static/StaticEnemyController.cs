using System.Collections;
using UnityEngine;

namespace Enemies.Static {
	[RequireComponent(typeof(ProjectileController))]

	public class StaticEnemyController : MonoBehaviour {
		public  float      range;
		public  float      cooldown;
		public  GameObject player;
		private bool       _shooting;
		public ProjectileController proj;

		void Update() {
			
			if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= range) {
				transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y,
				                             player.transform.position.z));

				if (!_shooting) StartCoroutine(nameof(Shoot));
			}
		}

		IEnumerator Shoot() {
			_shooting = true;
			proj = GetComponent<ProjectileController>();
			proj.shoot();
			yield return new WaitForSeconds(cooldown);
			_shooting = false;
		}
	}
}