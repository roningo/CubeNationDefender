using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using ThirdPersonShooter.Script.GOAP.Config;
using UnityEngine;

namespace ThirdPersonShooter.Script.GOAP.Sensors
{
    public class PlayerTargetSensor : LocalTargetSensorBase, IInjectable
    {
        private AttackConfigSO _attackConfig;
        private Collider[] _colliders = new Collider[1];

        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            if (Physics.OverlapSphereNonAlloc(agent.transform.position, _attackConfig.sensorRadius, _colliders,
                    _attackConfig.attackAbleLayerMask) > 0)
                return new TransformTarget(_colliders[0].transform);
            return null;
        }

        public void Inject(DependencyInjector injector)
        {
            _attackConfig = injector.attackConfig;
        }
    }
}