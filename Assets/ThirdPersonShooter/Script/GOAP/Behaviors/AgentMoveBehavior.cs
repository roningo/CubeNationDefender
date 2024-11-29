using System;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using ThirdPersonShooter.Script.GOAP.Goals;
using UnityEngine;
using UnityEngine.AI;

namespace ThirdPersonShooter.Script.GOAP.Behaviors
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(AgentBehaviour))]
    public class AgentMoveBehavior : MonoBehaviour
    {
        private static readonly int Walk = Animator.StringToHash("Walk");
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private AgentBehaviour _agentBehaviour;
        private ITarget _currentTarget;
        [SerializeField] private float _minMoveDistance = 0.25f;

        private Vector3 _lastPosition;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _agentBehaviour = GetComponent<AgentBehaviour>();
        }

        private void OnEnable()
        {
            _agentBehaviour.Events.OnTargetChanged += EventOnTargetChanged;
            _agentBehaviour.Events.OnTargetOutOfRange += EventOnTargetOutOfRange;
        }

        private void Update()
        {
            if (_currentTarget == null) return;

            if (_minMoveDistance <= Vector3.Distance(_currentTarget.Position, _lastPosition))
            {
                _lastPosition = _currentTarget.Position;
                _navMeshAgent.SetDestination(_currentTarget.Position);
            }

            _animator.SetBool(Walk, _navMeshAgent.velocity.magnitude > 0.1f);
        }

        private void EventOnTargetChanged(ITarget target, bool inrange)
        {
            _currentTarget = target;
            _lastPosition = _currentTarget.Position;

            NavMeshPath path = new();
            _navMeshAgent.CalculatePath(_currentTarget.Position, path);
            if (path.status != NavMeshPathStatus.PathComplete)
                _agentBehaviour.SetGoal<HomeReachGoal>(true);

            _navMeshAgent.SetDestination(_currentTarget.Position);

            _animator.SetBool(Walk, true);
        }

        private void EventOnTargetOutOfRange(ITarget target)
        {
            _animator.SetBool(Walk, false);
        }
    }
}