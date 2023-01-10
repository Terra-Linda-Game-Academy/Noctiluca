using System.Collections;
using UnityEngine;

namespace Enemies.Static {
	public class StaticEnemyController : MonoBehaviour {
		public  float      range;
		public  float      cooldown;
		public  GameObject player;
		public  GameObject projectilePrefab;
		private bool       _shooting;

		void Update() {
			if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= range) {
				transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y,
				                             player.transform.position.z));

				if (!_shooting) StartCoroutine(nameof(Shoot));
			}
		}

		IEnumerator Shoot() {
			_shooting = true;
			Instantiate(projectilePrefab, transform.position + 1f * transform.forward, transform.rotation);
			yield return new WaitForSeconds(cooldown);
			_shooting = false;
		}
	}
}