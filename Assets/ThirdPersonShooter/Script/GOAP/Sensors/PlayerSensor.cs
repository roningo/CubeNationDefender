using System;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using ThirdPersonShooter.Script.GOAP.Config;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace ThirdPersonShooter.Script.GOAP.Sensors
{
    public class PlayerSensor : MonoBehaviour
    {
        public SphereCollider sphereCollider;

        public UnityEvent<Transform> playerEnterEvent;
        public UnityEvent<Vector3> playerExitEvent;

        private void Awake()
        {
            sphereCollider = GetComponent<SphereCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                playerEnterEvent?.Invoke(other.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                playerExitEvent?.Invoke(other.transform.position);
        }
    }
}