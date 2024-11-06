using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomTrigger : MonoBehaviour
{
    public UnityEvent<Collider> EnteredTriggerEvent;
    public UnityEvent<Collider> StayTriggerEvent;
    public UnityEvent<Collider> ExitTriggerEvent;

    private void OnTriggerEnter(Collider other)
    {
        EnteredTriggerEvent?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        StayTriggerEvent?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        ExitTriggerEvent?.Invoke(other);
    }
}
