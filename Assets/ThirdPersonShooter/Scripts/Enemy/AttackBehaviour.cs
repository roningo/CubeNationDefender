using UnityEngine;

namespace ThirdPersonShooter.Scripts.Enemy
{
    public class AttackBehaviour : MonoBehaviour
    {
        [SerializeField] private MeleeHurtBox _meleeHurtBox;
        
        public void BeginAttack(int _)
        {
            _meleeHurtBox.gameObject.SetActive(true);
        }
        
        public void EndAttack(int _)
        {
            _meleeHurtBox.gameObject.SetActive(false);
        }
    }
}