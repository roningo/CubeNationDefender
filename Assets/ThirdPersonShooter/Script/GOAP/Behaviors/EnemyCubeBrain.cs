using CrashKonijn.Goap.Behaviours;
using ThirdPersonShooter.Script.BioStats;
using ThirdPersonShooter.Script.GOAP.Config;
using ThirdPersonShooter.Script.GOAP.Goals;
using ThirdPersonShooter.Script.GOAP.Sensors;
using UnityEngine;

namespace ThirdPersonShooter.Script.GOAP.Behaviors
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class EnemyCubeBrain : MonoBehaviour
    {
        [SerializeField] private PlayerSensor _playerSensor;
        [SerializeField] private MeleeHurtBox _meleeHurtBox;
        [SerializeField] private AttackConfigSO _attackConfig;
        private AgentBehaviour _agentBehaviour;

        private void Awake()
        {
            _agentBehaviour = GetComponent<AgentBehaviour>();
        }
        
        private void Start()
        {
            _agentBehaviour.SetGoal<HomeReachGoal>(false);
            
            _playerSensor.sphereCollider.radius = _attackConfig.sensorRadius;
        }

        private void OnEnable()
        {
            _playerSensor.playerEnterEvent.AddListener(PlayerSensorOnEnter);
            _playerSensor.playerExitEvent.AddListener(PlayerSensorOnExit);
            _meleeHurtBox.playerEnterEvent.AddListener(PlayerHit);
        }

        private void PlayerHit(Transform player)
        {
            player.GetComponent<PlayerHealth>().ReceivedDamage(10f);
        }

        private void OnDisable()
        {
            _playerSensor.playerEnterEvent.RemoveListener(PlayerSensorOnEnter);
            _playerSensor.playerExitEvent.RemoveListener(PlayerSensorOnExit);
        }

        private void PlayerSensorOnEnter(Transform player)
        {
            _agentBehaviour.SetGoal<KillPlayer>(true);
        }

        private void PlayerSensorOnExit(Vector3 playerPosition)
        {
            _agentBehaviour.SetGoal<HomeReachGoal>(true);
        }
    }
}