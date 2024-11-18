using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using ThirdPersonShooter.Script.GOAP.Config;
using UnityEngine;

namespace ThirdPersonShooter.Script.GOAP.Actions
{
    public class MeleeAction : ActionBase<AttackData>, IInjectable
    {
        private AttackConfigSO _attackConfig;

        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, AttackData data)
        {
            data.Timer = _attackConfig.attackDelay;
        } 

        public override ActionRunState Perform(IMonoAgent agent, AttackData data, ActionContext context)
        {
            data.Timer -= context.DeltaTime;

            bool shouldAttack = data.Target != null &&
                                Vector3.Distance(data.Target.Position, agent.transform.position)
                                <= _attackConfig.meleeRadius;
            data.Animator.SetBool(AttackData.Attack, shouldAttack);

            if (shouldAttack)
                agent.transform.LookAt(data.Target.Position);

            return data.Timer > 0
                ? ActionRunState.Continue
                : ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, AttackData data)
        {
            data.Animator.SetBool(AttackData.Attack, false);
        }

        public void Inject(DependencyInjector injector)
        {
            _attackConfig = injector.attackConfig;
        }
    }
}