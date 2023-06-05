using UnityEngine;
using Random = UnityEngine.Random;

namespace Misc {
    public class Stun : MonoBehaviour {
        private Vector3 pos;
        private Quaternion rot;

        private float timeRemaining;

        public float TimeRemaining {
            get => timeRemaining;
            set {
                timeRemaining = value;
                timeRemaining = Mathf.Max(0, timeRemaining);
                if (timeRemaining > 0) {
                    var tf = transform;
                    pos = tf.position;
                    rot = tf.rotation;
                }
            }
        }



        public void LateUpdate() {
            if (timeRemaining <= 0) {
                timeRemaining = 0;
                return;
            }

            var tf = transform;
            tf.position = pos + new Vector3(Random.Range(0, 0.1f), 0, Random.Range(0, 0.1f));
            tf.rotation = rot;

            timeRemaining -= Time.deltaTime;
        }
    }
}