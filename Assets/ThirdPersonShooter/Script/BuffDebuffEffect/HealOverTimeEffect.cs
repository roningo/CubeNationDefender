using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Heal Over Time Effect")]
public class HealOverTimeEffect : EffectScripable
{
    [Header("Effect stats")]
    public float heal;

    private Health _health;

    public override void OverrideSetup() {
        _health = activeMonoBehaviour.GetComponent<Health>();
    }

    public override void OverrideEffect() {
        _health.ReceivedHealing(heal);
    }
}



