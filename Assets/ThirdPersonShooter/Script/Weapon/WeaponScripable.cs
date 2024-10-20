using UnityEngine;

[CreateAssetMenu]
public class WeaponScripable : ScriptableObject
{
    //bullet
    public GameObject bullet;

    //bullet force
    public float fireForce, upwardForce;

    //weapon stats
    public float fireRate, spread, reloadTime, timeBetweenBrust;
    public int magazineSize, bulletPerTap;
    public bool allowHold;
    public bool allowGuideProjectile;

    //recoil
    public Rigidbody playerRb;
    public float recoilForce;

    //graphic
    public GameObject fireEffect;

    //ref
    public Transform firePoint;
}

