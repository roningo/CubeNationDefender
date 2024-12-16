using UnityEngine;

namespace ThirdPersonShooter.Scripts
{
    public class HomeHealth : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
                Destroy(other.gameObject);
        }
    }
}