using CrashKonijn.Goap.Behaviours;
using UnityEngine;

namespace ThirdPersonShooter.Script.GOAP.Behaviors
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class GoapSetBinder : MonoBehaviour
    {
        public GoapRunnerBehaviour goapRunner;

        private void Awake()
        {
            if(goapRunner)
            {
                AgentBehaviour agentBehaviour = GetComponent<AgentBehaviour>();
                agentBehaviour.GoapSet = goapRunner.GetGoapSet("EnemyCubeSet");
            }
        }
        
        public void Setup(GoapRunnerBehaviour goapRunner)
        {
            this.goapRunner = goapRunner;
            AgentBehaviour agentBehaviour = GetComponent<AgentBehaviour>();
            agentBehaviour.GoapSet = this.goapRunner.GetGoapSet("EnemyCubeSet");
        }
    }
}