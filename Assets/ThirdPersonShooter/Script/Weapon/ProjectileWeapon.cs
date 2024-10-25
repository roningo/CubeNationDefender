using System;
using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{
    public InputManager inputManager;

    //bullet
    [SerializeField] private GameObject _bullet;

    //bullet force
    [SerializeField] float _fireForce, _upwardForce;

    //weapon stats
    [SerializeField] private float _fireRate, _spread, _reloadTime, _timeBetweenBrust;
    [SerializeField] private int _magazineSize, _bulletPerTap;
    [SerializeField] private bool _allowHold;
    [SerializeField] private bool _allowGuideProjectile;

    int _bulletsLeft, _bulletShoot;

    //recoil
    [SerializeField] private Rigidbody _playerRb;
    [SerializeField] private float _recoilForce;

    //bools
    private bool _shooting, _readyToShoot, _reloading;

    //graphic
    [SerializeField] private GameObject _fireEffect;

    //ref
    [SerializeField] private Transform _firePoint;

    //
    private bool _allowInvoke = true;

    void Start()
    {
        //magazine full
        _bulletsLeft = _magazineSize;
        _readyToShoot = true;
    }

    void Update()
    {
        CheckReload();
    }

    //on changing weapon, disable old and enable new, OnEnable will run for re-initialization purposes 
    void OnEnable()
    {
        if (inputManager)
            inputManager.OnShoot.AddListener(ListenShoot);
    }

    void OnDisable()
    {
        if (inputManager)
            inputManager.OnShoot.RemoveListener(ListenShoot);
    }

    //cant RemoveListener(() => DetectShoot()) correctly, below solve the case
    void ListenShoot() => DetectShoot();

    public void DetectShoot(Vector3 targetPosition = new(), Action fireAnimate = null)
    {
        if (!_allowHold) inputManager.starterAssetsInputs.shoot = false;

        //shooting
        if (_readyToShoot && !_reloading && _bulletsLeft > 0)
        {
            //set bullet shoot to 0
            _bulletShoot = 0;

            fireAnimate?.Invoke();
            Shoot(targetPosition);

            CheckReload();

        }
    }

    private void Shoot(Vector3 targetPosition = new())
    {
        _readyToShoot = false;

        //check ray hit something
        Vector3 targetPoint;

        if (inputManager)
            targetPoint = inputManager.GetMouseWorldPosition();
        else
            targetPoint = targetPosition;

        //caculate direction
        Vector3 directionWithoutSpread = targetPoint - _firePoint.position;

        //caculate spread
        float x = UnityEngine.Random.Range(-_spread, _spread);
        float y = UnityEngine.Random.Range(-_spread, _spread);
        float z = UnityEngine.Random.Range(-_spread, _spread);

        //caculate direction
        Vector3 directionWithSpread = directionWithoutSpread.normalized + new Vector3(x, y, z);

        //spawn bullet
        GameObject currentBullet = Instantiate(_bullet, _firePoint.position, Quaternion.identity);
        //rotate bullet
        currentBullet.transform.forward = directionWithSpread.normalized;

        //add force
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * _fireForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(_firePoint.up * _upwardForce, ForceMode.Impulse);

        if (_allowGuideProjectile)
        {
            Vector3 guideDirection = targetPoint - currentBullet.transform.position;

            // currentBullet.transform.Translate(Vector3.Lerp(transform.forward, transform.forward, Time.deltaTime * 20f);
            // currentBullet.transform.LookAt(targetPoint);

        }

        //effect
        if (_fireEffect != null)
        {
            Instantiate(_fireEffect, _firePoint.position, Quaternion.identity);
        }

        //invoke reset
        if (_allowInvoke)
        {
            Invoke(nameof(ResetShoot), 1 / _fireRate);
            _allowInvoke = false;

            //add recoil
            // playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
        }

        _bulletsLeft--;
        _bulletShoot++;

        //if more bullet per tap
        if (_bulletShoot < _bulletPerTap && _bulletsLeft > 0)
        {
            //_timeBetweenBrust = 0 but still delay a bit
            if (_timeBetweenBrust == 0)
                Shoot(targetPosition);
            else
                this.Wait(_timeBetweenBrust, () => Shoot(targetPosition));

        }
    }

    private void ResetShoot()
    {
        _readyToShoot = true;
        _allowInvoke = true;
    }

    private void CheckReload()
    {
        //reloading 
        if (_bulletsLeft < _magazineSize && !_reloading && false) //need press reload
            Reload();

        //auto reload
        if (_bulletsLeft <= 0 && !_reloading) Reload();

    }

    private void Reload()
    {
        _reloading = true;
        Invoke(nameof(ReloadFinished), _reloadTime);
    }

    private void ReloadFinished()
    {
        _bulletsLeft = _magazineSize;
        _reloading = false;
    }
}
