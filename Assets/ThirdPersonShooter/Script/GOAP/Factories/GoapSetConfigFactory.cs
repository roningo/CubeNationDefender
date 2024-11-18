using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Resolver;
using ThirdPersonShooter.Script.GOAP.Actions;
using ThirdPersonShooter.Script.GOAP.Goals;
using ThirdPersonShooter.Script.GOAP.Sensors;
using ThirdPersonShooter.Script.GOAP.Targets;
using ThirdPersonShooter.Script.GOAP.Worldkeys;
using UnityEngine;

namespace ThirdPersonShooter.Script.GOAP.Factories
{
    [RequireComponent(typeof(DependencyInjector))]
    public class GoapSetConfigFactory : GoapSetFactoryBase
    {
        private DependencyInjector _injector;

        public override IGoapSetConfig Create()
        {
            _injector = GetComponent<DependencyInjector>();
            GoapSetBuilder builder = new("EnemyCubeSet");

            BuildGoals(builder);
            BuildActions(builder);
            BuildSensors(builder);

            return builder.Build();
        }

        private void BuildGoals(GoapSetBuilder builder)
        {
            builder.AddGoal<HomeReachGoal>()
                .AddCondition<IsHomeReaching>(Comparison.GreaterThan, 1);

            builder.AddGoal<KillPlayer>()
                .AddCondition<PlayerHealth>(Comparison.SmallerThanOrEqual, 0);
        }

        private void BuildActions(GoapSetBuilder builder)
        {
            builder.AddAction<HomeReachAction>()
                .SetTarget<HomeReachTarget>()
                .AddEffect<IsHomeReaching>(EffectType.Increase)
                .SetBaseCost(5)
                .SetInRange(10);

            builder.AddAction<MeleeAction>()
                .SetTarget<PlayerTarget>()
                .AddEffect<PlayerHealth>(EffectType.Decrease)
                .SetBaseCost(_injector.attackConfig.meleeCost)
                .SetInRange(_injector.attackConfig.sensorRadius);
        }

        private void BuildSensors(GoapSetBuilder builder)
        {
            builder.AddTargetSensor<HomeReachTargetSensor>()
                .SetTarget<HomeReachTarget>();

            builder.AddTargetSensor<PlayerTargetSensor>()
                .SetTarget<PlayerTarget>();
        }
    }
}