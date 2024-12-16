namespace ThirdPersonShooter.Scripts.BioStats
{
    public interface IHealth
    {
        void ReceivedDamage(float damage);
        void ReceivedHealing(float heal);
        void OnDeath();
    }
}