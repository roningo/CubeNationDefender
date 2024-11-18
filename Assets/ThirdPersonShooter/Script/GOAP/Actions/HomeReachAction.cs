using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Random = UnityEngine.Random;

namespace ThirdPersonShooter.Script.GOAP.Actions
{
    public class HomeReachAction : ActionBase<CommonData>
    {
        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, CommonData data)
        {
            data.Timer = Random.Range(1, 2);
        }

        public override ActionRunState Perform(IMonoAgent agent, CommonData data, ActionContext context)
        {
            data.Timer = context.DeltaTime;

            return data.Timer > 0
                ? ActionRunState.Continue
                : ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, CommonData data)
        {
        }
    }
}