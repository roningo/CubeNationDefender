using System;
using UnityEngine;

public class CustomProjectile : MonoBehaviour
{
    //assignable
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask enemyLayer;

    //stats
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    //damage
    public float damage;
    public float explosionRange;
    public float explosionForce;

    //Lifetime  
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnImpact = true;

    private int collisions;
    private bool _triggered = false;

    public EffectScripable effectScripable;

    void Start()
    {
        if (TryGetComponent<TrailRenderer>(out TrailRenderer trailRenderer))
        {
            trailRenderer.time = 0.1f; //life time
            trailRenderer.startWidth = 0.05f; //spawn width
            trailRenderer.endWidth = 0f; //end width
        }
        Setup();
    }

    void Update()
    {
        transform.forward = rb.velocity;
        //collisions up to explode
        if (maxCollisions != 0 && collisions >= maxCollisions) DamageDeal();

        //count down lifetime
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0) DamageDeal();
    }

    private void DamageDeal(Collision other = null)
    {
        if (!_triggered)
        {
            _triggered = true;

            //instantiate explode
            if (explosion) Instantiate(explosion, transform.position, Quaternion.identity);

            if (other != null && explosionRange <= 0) //hit-type projectile
            {
                if (other.gameObject.TryGetComponent<Health>(out Health health))
                    health.ReceivedDamage(damage);
                ProjectileEffect(other.collider);
            }
            else //explode-type projectile
            {
                //check enemies in explosionRange
                Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, enemyLayer);

                foreach (Collider e in enemies)
                {
                    //get emnemies component to call damage
                    if (e.TryGetComponent<Health>(out Health health))
                        health.ReceivedDamage(damage);
                    ProjectileEffect(e);

                    if (e.TryGetComponent<Rigidbody>(out Rigidbody erb))
                        erb.AddExplosionForce(explosionForce, transform.position, explosionRange);
                }
            }

            Invoke(nameof(DelayDestroy), float.MinValue);
        }
    }

    private void ProjectileEffect(Collider other)
    {
        if (other.gameObject.TryGetComponent<EffectSystem>(out EffectSystem effectSystem) && effectScripable)
        {
            EffectScripable currentEffect = effectScripable;
            effectSystem.AddEffect(currentEffect);
        }
    }

    private void DelayDestroy()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision other)
    {
        if (explodeOnImpact) DamageDeal(other);
        else if (other.collider.CompareTag("Enemy"))
            collisions++; //count collisions
    }

    private void Setup()
    {
        //create new physic material
        PhysicMaterial physic_mat = new()
        {
            bounciness = this.bounciness,
            frictionCombine = PhysicMaterialCombine.Minimum,
            bounceCombine = PhysicMaterialCombine.Maximum
        };

        //assign to collider
        GetComponent<Collider>().material = physic_mat;

        //set gravity
        rb.useGravity = useGravity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
