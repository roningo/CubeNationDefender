using UnityEngine;

namespace ThirdPersonShooter.Scripts
{
    public static class ObjectPositionFunction
    {
        public static Vector3 GetCenterOrPosition(GameObject gameObject)
        {
            Vector3 centerOrPosition = gameObject.TryGetComponent<Collider>(out Collider coll)
                ? coll.bounds.center
                : gameObject.transform.position;

            return centerOrPosition;
        }
    }
}