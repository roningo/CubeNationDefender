using CrashKonijn.Goap.Behaviours;
using UnityEngine;

namespace ThirdPersonShooter.Script.GOAP.Behaviors
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class GoapSetBinder : MonoBehaviour
    {
        [SerializeField] private GoapRunnerBehaviour _goapRunner;

        private void Awake()
        {
            AgentBehaviour agentBehaviour = GetComponent<AgentBehaviour>();
            agentBehaviour.GoapSet = _goapRunner.GetGoapSet("EnemyCubeSet");
        }
    }
}