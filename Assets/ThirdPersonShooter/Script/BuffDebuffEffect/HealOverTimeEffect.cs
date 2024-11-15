using ThirdPersonShooter.Script.Enemy;
using UnityEngine;

namespace ThirdPersonShooter.Script.BuffDebuffEffect
{
    [CreateAssetMenu(menuName = "Effect/Heal Over Time Effect")]
    public class HealOverTimeEffect : EffectScripable
    {
        [Header("Effect stats")]
        public float heal;

        private Health _health;

        protected override void OverrideSetup() {
            _health = activeMonoBehaviour.GetComponent<Health>();
        }

        protected override void OverrideEffect() {
            _health.ReceivedHealing(heal);
        }
    }
}



