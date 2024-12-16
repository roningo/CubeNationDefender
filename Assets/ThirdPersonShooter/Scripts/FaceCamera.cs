using UnityEngine;

namespace ThirdPersonShooter.Scripts
{
    public class FaceCamera : MonoBehaviour
    {
        public Camera faceCamera;

        private void Update()
        {
            if (faceCamera)
                transform.LookAt(faceCamera.transform, Vector3.up);
        }
    }
}