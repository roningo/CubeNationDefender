using UnityEngine;
using UnityEngine.AI;

public class FindDestination : MonoBehaviour
{
    [SerializeField] private Transform _sightPoint;
    [SerializeField] private float _sightRadius = 5f;
    [SerializeField] private LayerMask _sightLayer;

    public Transform homeDestination;
    private NavMeshAgent _ai;

    private Transform _currentTarget;

    private Collider[] _targetList = new Collider[1];
    private int _targetCount;

    // Start is called before the first frame update
    void Start()
    {
        _currentTarget = homeDestination;
        _ai = GetComponent<NavMeshAgent>();
        SetDestination();
    }

    // Update is called once per frame
    void Update()
    {
        _targetCount = Physics.OverlapSphereNonAlloc(_sightPoint.position, _sightRadius, _targetList, _sightLayer);

        if (_targetCount > 0)
        {
            foreach (Collider collapseObject in _targetList)
            {
                if (collapseObject == null || !collapseObject.gameObject.CompareTag("Player")) continue;

                NavMeshPath path = new();
                _currentTarget = collapseObject.gameObject.transform;
                _ai.CalculatePath(_currentTarget.gameObject.transform.position, path);
                if (path.status != NavMeshPathStatus.PathComplete)
                    _currentTarget = homeDestination;
                SetDestination();
            }
        }
        else if (_currentTarget != homeDestination)
        {
            _currentTarget = homeDestination;
            SetDestination();
        }
    }

    private void SetDestination()
    {
        _ai.SetDestination(_currentTarget.position);
    }
}