using ThirdPersonShooter.Script.BioStats;
using UnityEngine;
using UnityEngine.Events;

namespace ThirdPersonShooter.Script.GOAP.Sensors
{
    public class MeleeHurtBox : MonoBehaviour
    {
        public UnityEvent<Transform> playerEnterEvent;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerEnterEvent?.Invoke(other.transform);
                other.GetComponent<PlayerHealth>().ReceivedDamage(10f);

                gameObject.SetActive(false);
            }
        }
    }
}