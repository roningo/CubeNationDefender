using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CustomProjectileTest : MonoBehaviour
{
    //assignable
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask enemyLayer;

    //stats
    [Range(0f, 1f)] public float bounciness;
    public bool useGravity;

    //damage
    public float damage;
    public float explosionRange;
    public float explosionForce;

    //Lifetime  
    public int maxCollisions;
    public float maxLifetime;
    public ProjectileMode projectileMode = ProjectileMode.Impact;

    private int _collisions;
    private bool _triggered;

    public EffectScripable effect;

    private Collider _impactCollision;

    private bool _isImpact;
    private bool _isCollisionMax;
    private bool _isOutOfTime;

    private readonly Stopwatch _durationTimer = new();

    public enum ProjectileMode
    {
        Impact,
        Bounce,
        Penetrate,
    }

    private void Start()
    {
        if (TryGetComponent<TrailRenderer>(out TrailRenderer trailRenderer))
        {
            trailRenderer.time = 0.1f; //life time
            trailRenderer.startWidth = 0.05f; //spawn width
            trailRenderer.endWidth = 0f; //end width
        }

        //create new physic material
        PhysicMaterial physicMat = new()
        {
            bounciness = this.bounciness,
            frictionCombine = PhysicMaterialCombine.Minimum,
            bounceCombine = PhysicMaterialCombine.Maximum
        };

        //assign to collider
        GetComponent<Collider>().material = physicMat;

        if (projectileMode == ProjectileMode.Penetrate)
            if (TryGetComponent<Collider>(out Collider selfCollider))
                selfCollider.isTrigger = true;

        //set gravity
        rb.useGravity = useGravity;

        //set Timer
        _durationTimer.Start();
    }

    private void Update()
    {
        transform.forward = rb.velocity;
        if (CheckReadyToDamaged()) DamageDeal(_impactCollision);
    }

    private bool CheckReadyToDamaged()
    {
        _isImpact = _impactCollision;
        _isCollisionMax = maxCollisions != 0 && _collisions >= maxCollisions;
        _isOutOfTime = _durationTimer.IsRunning && _durationTimer.Elapsed.TotalSeconds >= maxLifetime;

        return _isImpact || _isCollisionMax || _isOutOfTime;
    }

    private void DamageDeal(Collider other = null)
    {
        if (_triggered) return;
        _triggered = true;
        _impactCollision = null;

        //instantiate explode
        if (explosion) Instantiate(explosion, transform.position, Quaternion.identity);

        if (other && explosionRange <= 0) //hit-type projectile
        {
            if (other.gameObject.TryGetComponent<Health>(out Health health))
                health.ReceivedDamage(damage);
            ProjectileEffect(other);
        }
        else //explode-type projectile
        {
            //check enemies in explosionRange
            Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, enemyLayer);
            foreach (Collider e in enemies)
            {
                //get enemies component to call damage
                if (e.TryGetComponent<Health>(out Health health))
                    health.ReceivedDamage(damage);
                ProjectileEffect(e);

                //knock back
                if (e.TryGetComponent<Rigidbody>(out Rigidbody erb))
                    erb.AddExplosionForce(explosionForce, transform.position, explosionRange);
            }
        }

        Invoke(nameof(DelayDestroy), float.MinValue);
    }

    private void ProjectileEffect(Collider other)
    {
        if (!other.gameObject.TryGetComponent<EffectSystem>(out EffectSystem effectSystem) || !effect) return;

        EffectScripable currentEffect = effect;
        effectSystem.AddEffect(currentEffect);
    }

    private void DelayDestroy()
    {
        _triggered = false;
        if (projectileMode != ProjectileMode.Penetrate
        || _isOutOfTime)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!_impactCollision)
            _impactCollision = other.collider;

        if (other.collider.CompareTag("Enemy"))
            _collisions++; //count collisions
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}