using System;
using Unity.Cinemachine;
using UnityEngine;

public class YouShallNotPass : MonoBehaviour
{
    [SerializeField] private GameObject respawnPoint;
    [SerializeField] private Camera mainCamera;

    private void OnTriggerStay(Collider other)
    {
        other.gameObject.SetActive(false);
        other.transform.position = respawnPoint.transform.position;
        other.gameObject.SetActive(true);
    }
}