using System;
using UnityEngine;
using UnityEngine.AI;

public class TargetDetector : MonoBehaviour
{
    [SerializeField] private float _detectionRadius = 5f;
    [SerializeField] private LayerMask _detectionLayer;
    [SerializeField] private GameObject _homeDestination;

    private NavMeshAgent _ai;
    private GameObject _targetDestination;

    private void Awake()
    {
        _ai = GetComponent<NavMeshAgent>();
    }

    public void SetupHome(GameObject home)
    {
        _homeDestination = home;
    }

    public GameObject UpdateDetector()
    {
        Collider[] colliders = new Collider[1];
        int colliderCount =
            Physics.OverlapSphereNonAlloc(transform.position, _detectionRadius, colliders, _detectionLayer);
        _targetDestination = colliderCount > 0 ? colliders[0].gameObject : _homeDestination;


        if (colliderCount > 0)
        {
            _targetDestination = colliders[0].gameObject;
            NavMeshPath path = new();
            _ai.CalculatePath(_targetDestination.transform.position, path);
            if (path.status != NavMeshPathStatus.PathComplete)
                _targetDestination = _homeDestination;
        }
        else _targetDestination = _homeDestination;

        return _targetDestination;
    }
}