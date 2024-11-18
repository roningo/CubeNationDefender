using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;

namespace ThirdPersonShooter.Script.GOAP.Sensors
{
    public class HomeReachTargetSensor : LocalTargetSensorBase, IInjectable
    {
        private Transform _homeDestination;

        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            Vector3 pos = _homeDestination.position;
            return new PositionTarget(pos);
        }

        public void Inject(DependencyInjector injector)
        {
            _homeDestination = injector.homeDestination;
        }
    }
}