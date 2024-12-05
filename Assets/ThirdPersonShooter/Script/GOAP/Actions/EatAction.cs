using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using ThirdPersonShooter.Script.GOAP.Config;
using UnityEngine;

namespace ThirdPersonShooter.Script.GOAP.Actions
{
    public class EatAction : ActionBase<EatAction.Data>, IInjectable
    {
        public class Data : CommonData
        {
            private float hunger {set;get;}
        }

        private AttackConfigSO _attackConfig;

        public override void Created()
        {
            throw new System.NotImplementedException();
        }

        public override void Start(IMonoAgent agent, Data data)
        {
            throw new System.NotImplementedException();
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            throw new System.NotImplementedException();
        }

        public override void End(IMonoAgent agent, Data data)
        {
            throw new System.NotImplementedException();
        }

        public void Inject(DependencyInjector injector)
        {
            throw new System.NotImplementedException();
        }
    }
}