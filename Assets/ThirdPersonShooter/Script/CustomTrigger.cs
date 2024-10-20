using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomTrigger : MonoBehaviour
{
    public UnityEvent<Collider> EnteredTriggerEvent;
    public UnityEvent<Collider> StayTriggerEvent;
    public UnityEvent<Collider> ExitTriggerEvent;

    void OnTriggerEnter(Collider other)
    {
        EnteredTriggerEvent?.Invoke(other);
    }

    void OnTriggerStay(Collider other)
    {
        StayTriggerEvent?.Invoke(other);
    }

    void OnTriggerExit(Collider other)
    {
        ExitTriggerEvent?.Invoke(other);
    }
}
