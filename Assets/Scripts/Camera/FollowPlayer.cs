using UnityEngine;

namespace Camera
{
    public class FollowPlayer : MonoBehaviour
    {

        /// <summary>
        /// 
        /// just using this as a test for my playercontroller
        /// 
        /// pretty sure we aren't using cinemachine so why not
        /// 
        /// </summary>

        public GameObject player;
        private Quaternion cameraRotation;
        private Vector3 transf;

        void Start() {
            transf         = transform.position - player.transform.position;
            cameraRotation = transform.rotation;
        }


        void LateUpdate()
        {
            transform.position = player.transform.position + transf;
            transform.rotation = cameraRotation;
        }
    }
}
