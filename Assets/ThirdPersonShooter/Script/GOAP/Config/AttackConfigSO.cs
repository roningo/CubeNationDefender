using UnityEngine;

namespace ThirdPersonShooter.Script.GOAP.Config
{
    [CreateAssetMenu(menuName = "Ai/Attack Config")]

    public class AttackConfigSO : ScriptableObject
    {
        public float sensorRadius = 5f;
        public float meleeRadius = 1f;
        public int meleeCost = 1;
        public float meleeDamage = 10f;
        public float attackDelay = 1f;
        public LayerMask attackAbleLayerMask;
    }
}