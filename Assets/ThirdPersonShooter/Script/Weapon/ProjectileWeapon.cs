using System;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] private bool _allowGuideBullet;

    int _bulletsLeft, _bulletShoot;

    //recoil
    [SerializeField] private Rigidbody _playerRb;
    [SerializeField] private float _recoilForce;

    //bools
    private bool _shooting, _readyToShoot, _reloading;

    //graphic
    [SerializeField] private GameObject _fireEffect;

    //ref
    public Transform firePoint;

    public ShootMode shootMode = ShootMode.DIRECT;

    public enum ShootMode
    {
        DIRECT,
        THROW,
    }

    [Range(0, 1)]
    [Tooltip("Using a values closer to 0 will make the agent throw with the lower force"
             + "down to the least possible force (highest angle) to reach the target.\n"
             + "Using a value of 1 the agent will always throw with the MaxThrowForce below.")]
    [SerializeField]
    private float _forceRatio = 0;

    public bool isMovementPrediction;

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
    void ListenShoot() => DetectShoot(inputManager ? inputManager.GetMouseWorldGameObject() : new GameObject());

    public void DetectShoot(GameObject targetObject, Action fireAnimate = null)
    {
        if (!_allowHold) inputManager.starterAssetsInputs.shoot = false;

        //shooting
        if (!_readyToShoot || _reloading || _bulletsLeft <= 0) return;

        //set bullet shoot to 0
        _bulletShoot = 0;

        fireAnimate?.Invoke();
        Shoot(targetObject);

        CheckReload();
    }

    private void Shoot(GameObject targetObject)
    {
        Vector3 targetPosition = targetObject.transform.position;
        _readyToShoot = false;

        //spawn bullet
        GameObject currentBullet = Instantiate(_bullet, firePoint.position, Quaternion.identity);

        switch (shootMode)
        {
            case ShootMode.DIRECT:
                DirectShoot(currentBullet, targetObject);
                break;
            case ShootMode.THROW:
                ThrowShoot(currentBullet, targetObject);
                break;
        }


        //effect
        if (_fireEffect)
            Instantiate(_fireEffect, firePoint.position, Quaternion.identity);

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
                Shoot(targetObject);
            else
                this.Wait(_timeBetweenBrust, () => Shoot(targetObject));
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


    public void DirectShoot(GameObject currentBullet, GameObject targetObject)
    {
        Vector3 targetPosition = targetObject.transform.position;
        //caculate direction
        Vector3 directionWithoutSpread = targetPosition - firePoint.position;

        if (isMovementPrediction)
            directionWithoutSpread = PredictedShootData(directionWithoutSpread, targetObject);

        //caculate spread
        float x = UnityEngine.Random.Range(-_spread, _spread);
        float y = UnityEngine.Random.Range(-_spread, _spread);
        float z = UnityEngine.Random.Range(-_spread, _spread);

        //caculate direction
        Vector3 directionWithSpread = directionWithoutSpread.normalized + new Vector3(x, y, z);

        Vector3 bulletVelocity = directionWithSpread.normalized * _fireForce + firePoint.up * _upwardForce;
        Shoot(bulletVelocity, currentBullet);
    }

    private Vector3 PredictedShootData(Vector3 directShootData, GameObject targetObject)
    {
        Vector3 shootVelocity = directShootData * _fireForce;
        shootVelocity.y = 0;
        Vector3 targetPosition = targetObject.transform.position;
        Vector3 displacement = targetPosition - firePoint.position;
        float time = displacement.magnitude / shootVelocity.magnitude;
        Vector3 targetMovement = new();

        if (targetObject.TryGetComponent<NavMeshAgent>(out NavMeshAgent targetAgent))
            targetMovement = targetAgent.velocity * time;

        Vector3 newTargetPosition = new Vector3(
            targetPosition.x + targetMovement.x,
            targetPosition.y + targetMovement.y,
            targetPosition.z + targetMovement.z
        );

        return newTargetPosition - firePoint.position;
    }

    private void Shoot(Vector3 bulletVelocity, GameObject currentBullet)
    {
        if (currentBullet.TryGetComponent<Rigidbody>(out Rigidbody rb))
            rb.AddForce(bulletVelocity, ForceMode.Impulse);
        else
            Debug.LogError("Projectile Rigidbody not found");
    }


    public void ThrowShoot(GameObject currentBullet, GameObject targetObject)
    {
        ThrowData throwData = GetThrowData(firePoint.position, targetObject);
        Vector3 bulletVelocity = throwData.ThrowVelocity;

        Throw(bulletVelocity, currentBullet);
    }

    public ThrowData GetThrowData(Vector3 firePoint, GameObject targetObject)
    {
        ThrowData throwData = CalculateThrowData(
            targetObject.transform.position,
            firePoint
        );

        if (isMovementPrediction)
            throwData = PredictedThrowData(throwData, targetObject);

        return throwData;
    }

    private ThrowData PredictedThrowData(ThrowData directThrowData, GameObject targetObject)
    {
        Vector3 throwVelocity = directThrowData.ThrowVelocity;
        throwVelocity.y = 0;
        float time = directThrowData.DeltaXZ / throwVelocity.magnitude;
        Vector3 targetPosition = targetObject.transform.position;
        Vector3 targetMovement = new();

        if (targetObject.TryGetComponent<NavMeshAgent>(out NavMeshAgent targetAgent))
            targetMovement = targetAgent.velocity * time;

        Vector3 newTargetPosition = new Vector3(
            targetPosition.x + targetMovement.x,
            targetPosition.y + targetMovement.y,
            targetPosition.z + targetMovement.z
        );

        // Option Calculate again the trajectory based on target position
        ThrowData predictiveThrowData = CalculateThrowData(
            newTargetPosition,
            firePoint.position
        );

        predictiveThrowData.ThrowVelocity = Vector3.ClampMagnitude(
            predictiveThrowData.ThrowVelocity,
            _fireForce
        );

        return predictiveThrowData;
    }

    private void Throw(Vector3 bulletVelocity, GameObject currentBullet)
    {
        if (currentBullet.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.AddForce(bulletVelocity, ForceMode.Impulse);
        }
        else
            Debug.LogError("Projectile Rigidbody not found");
    }

    private ThrowData CalculateThrowData(Vector3 targetPosition, Vector3 startPosition)
    {
        // v = initial velocity, assume max speed for now
        // x = distance to travel on X/Z plane only
        // y = difference in altitudes from thrown point to target hit point
        // g = gravity

        Vector3 displacement = new Vector3(
            targetPosition.x,
            startPosition.y,
            targetPosition.z
        ) - startPosition;
        float deltaY = targetPosition.y - startPosition.y;
        float deltaXZ = displacement.magnitude;

        // find lowest initial launch velocity with other magic formula from https://en.wikipedia.org/wiki/Projectile_motion
        // v^2 / g = y + sqrt(y^2 + x^2)
        // meaning.... v = sqrt(g * (y+ sqrt(y^2 + x^2)))
        float gravity = Mathf.Abs(Physics.gravity.y);
        float throwStrength = Mathf.Clamp(
            Mathf.Sqrt(
                gravity
                * (deltaY + Mathf.Sqrt(Mathf.Pow(deltaY, 2)
                                       + Mathf.Pow(deltaXZ, 2)))),
            0.01f,
            _fireForce
        );
        throwStrength = Mathf.Lerp(throwStrength, _fireForce, _forceRatio);

        float angle;
        if (_forceRatio == 0)
        {
            // optimal angle is chosen with a relatively simple formula
            angle = Mathf.PI / 2f - (0.5f * (Mathf.PI / 2 - (deltaY / deltaXZ)));
        }
        else
        {
            // when we know the initial velocity, we have to calculate it with this formula
            // Angle to throw = arctan((v^2 +- sqrt(v^4 - g * (g * x^2 + 2 * y * v^2)) / g*x)
            // + give the higher angle
            angle = Mathf.Atan(
                (Mathf.Pow(throwStrength, 2) + Mathf.Sqrt( //or this: (Mathf.Pow(throwStrength, 2) - Mathf.Sqrt(
                    Mathf.Pow(throwStrength, 4) - gravity
                    * (gravity * Mathf.Pow(deltaXZ, 2)
                       + 2 * deltaY * Mathf.Pow(throwStrength, 2)))
                ) / (gravity * deltaXZ)
            );
        }

        if (float.IsNaN(angle))
            // you will need to handle this case when there
            // is no feasible angle to throw the object and reach the target.
            return new ThrowData();

        Vector3 initialVelocity =
            Mathf.Cos(angle) * throwStrength * displacement.normalized
            + Mathf.Sin(angle) * throwStrength * Vector3.up;

        return new ThrowData
        {
            ThrowVelocity = initialVelocity,
            Angle = angle,
            DeltaXZ = deltaXZ,
            DeltaY = deltaY
        };
    }

    public struct ThrowData
    {
        public Vector3 ThrowVelocity;
        public float Angle;
        public float DeltaXZ;
        public float DeltaY;
    }
}