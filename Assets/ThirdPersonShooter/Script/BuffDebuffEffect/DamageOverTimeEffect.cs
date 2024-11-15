using ThirdPersonShooter.Script.Enemy;
using UnityEngine;

namespace ThirdPersonShooter.Script.BuffDebuffEffect
{
    [CreateAssetMenu(menuName = "Effect/Damage Over Time Effect")]
    public class DamageOverTimeEffect : EffectScripable
    {
        [Header("Effect stats")]
        public float damage;

        private Health _health;

        protected override void OverrideSetup() {
            _health = activeMonoBehaviour.GetComponent<Health>();
        }

        protected override void OverrideEffect() {
            _health.ReceivedDamage(damage);
        }
    }
}



