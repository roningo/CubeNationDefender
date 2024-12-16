using UnityEngine;
using UnityEngine.Events;

namespace ThirdPersonShooter.Scripts
{
    public class CustomTrigger : MonoBehaviour
    {
        public UnityEvent<Collider> enteredTriggerEvent;
        public UnityEvent<Collider> stayTriggerEvent;
        public UnityEvent<Collider> exitTriggerEvent;

        private void OnTriggerEnter(Collider other)
        {
            enteredTriggerEvent?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            stayTriggerEvent?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            exitTriggerEvent?.Invoke(other);
        }
    }
}