using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
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
        _ai.stoppingDistance = 2.5f;
        SetDestination();
    }

    // Update is called once per frame
    void Update()
    {
        _targetCount = Physics.OverlapSphereNonAlloc(transform.position, _sightRadius, _targetList, _sightLayer);

        if (_targetCount > 0)
        {
            foreach (Collider collapseObject in _targetList)
            {
                if (!collapseObject || !collapseObject.gameObject.CompareTag("Player")) continue;

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _sightRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, 1);
    }
}