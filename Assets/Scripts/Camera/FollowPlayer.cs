using UnityEngine;

namespace Camera {
    public class FollowPlayer : MonoBehaviour
    {

        /// <summary>
        /// 
        /// just using this as a test for my playercontroller
        /// 
        /// pretty sure we aren't using cinemachine so why not
        /// 
        /// </summary>

        private GameObject player;
        public Quaternion cameraRotation;
        public Vector3 transf;

        void Start()
        {
            player = GameObject.Find("Player");
            cameraRotation = transform.rotation;
        }


        void LateUpdate()
        {
            transform.position = player.transform.position + transf;
            transform.rotation = cameraRotation;
        }
    }
}
