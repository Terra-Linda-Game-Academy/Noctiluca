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

        public GameObject player;
        public Vector3 transf;

        void Start()
        {
            player = GameObject.Find("Player");
        }


        void Update()
        {
            transform.position = player.transform.position + transf;
        }
    }
}
