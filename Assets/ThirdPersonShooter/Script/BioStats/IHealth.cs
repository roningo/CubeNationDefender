namespace ThirdPersonShooter.Script.BioStats
{
    public interface IHealth
    {
        void ReceivedDamage(float damage);
        void ReceivedHealing(float heal);
        void OnDeath();
    }
}