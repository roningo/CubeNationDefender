using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Damage Over Time Effect")]
public class DamageOverTimeEffect : EffectScripable
{
    [Header("Effect stats")]
    public float damage;

    private Health _health;

    public override void OverrideSetup() {
        _health = activeMonoBehaviour.GetComponent<Health>();
    }

    public override void OverrideEffect() {
        _health.ReceivedDamage(damage);
    }
}


