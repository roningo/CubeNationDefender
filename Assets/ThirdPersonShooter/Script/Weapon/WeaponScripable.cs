using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.Jobs;

[CreateAssetMenu]
public class WeaponScripable : ScriptableObject
{
    //setup position
    public Vector3 spawnPoint;
    public Vector3 spawnRotation;
    public Vector3 spawnScale;

    //model
    public GameObject weaponPf;
    public GameObject bulletPf;

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

    private MonoBehaviour _activeMonoBehaviour;
    private GameObject _model;

    // public void Spawn(Transform parent, MonoBehaviour activeMonoBehaviour) {
    //     this._activeMonoBehaviour = activeMonoBehaviour;

    //     _model = Instantiate(weaponPf);
    //     _model.transform.SetParent(parent, false);
    //     _model.transform.SetLocalPositionAndRotation(spawnPoint, Quaternion.Euler(spawnRotation));
    //     _model.transform.localScale = spawnScale;
    // }
}

