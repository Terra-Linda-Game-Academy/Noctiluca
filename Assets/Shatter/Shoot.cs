using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

		public GameObject Projectile; 
		public float MinDelay = 0.25f;
		public float InitialSpeed = 10.0f;
		public Transform SpawnLocation;

		float lastShot = -1000.0f;

		void Update() {
			var shooting = UnityEngine.Input.GetMouseButtonDown(0);

			if (shooting) {
				if (Time.time - lastShot >= MinDelay) {
					lastShot = Time.time;

					var go = Instantiate(Projectile, SpawnLocation.position, SpawnLocation.rotation);

					go.GetComponent<Rigidbody>().velocity = InitialSpeed * UnityEngine.Camera.main.transform.forward;
				}
			} 
		}
	}