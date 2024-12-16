using UnityEngine;

namespace ThirdPersonShooter.Scripts
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 1.3f, 0f);

        private void Update()
        {
            if (target)
                transform.position = target.position + offset;
        }
    }
}