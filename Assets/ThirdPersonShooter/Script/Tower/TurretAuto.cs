using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretAuto : ProjectileWeapon
{
    [Header("Turret stats")] public CustomTrigger attackRageTrigger;

    [SerializeField] private GameObject _core, _barrel;
    [SerializeField] private float _turningSpeed = 20f;

    [SerializeField] private float _fireRange = 7f;
    private Animator _fireAnimate;

    private List<GameObject> enemiesInRange = new();
    private GameObject currentTarget;

    public bool autoMode = true;

    // private bool _isReloading = false;
    public bool canFire = true;

    private bool _hasAnimator;

    private void Awake()
    {
        attackRageTrigger.EnteredTriggerEvent.AddListener(OnAttackTriggerEnter);
        attackRageTrigger.ExitTriggerEvent.AddListener(OnAttackTriggerExit);

        Transform rangeFind = this.gameObject.transform.Find("Range");
        if (rangeFind && rangeFind.TryGetComponent<SphereCollider>(out SphereCollider sphereResult))
            sphereResult.radius = _fireRange;
        // _fireAnimate = GetComponent<Animator>();
        _hasAnimator = TryGetComponent<Animator>(out _fireAnimate);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!autoMode) return;

        if (enemiesInRange.Any(e => !e))
        {
            enemiesInRange.RemoveAll(e => !e);
            UpdateTarget();
        }

        if (!currentTarget) return;

        AimDirection(currentTarget);
        TriggerShoot(currentTarget);
    }

    private void OnAttackTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy")) return;

        enemiesInRange.Add(other.gameObject);
        UpdateTarget();
    }

    private void OnAttackTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy")) return;

        enemiesInRange.Remove(other.gameObject);
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        if (currentTarget && enemiesInRange.Contains(currentTarget)) return;
        currentTarget = enemiesInRange.Count > 0 ? enemiesInRange.First() : null;
    }

    // public void AimDirection(GameObject currentTarget)
    // {
    //     ThrowData throwData = GetThrowData(firePoint.position, currentTarget);
    //     Vector3 currentTargetPosition = currentTarget.transform.position;
    //     Vector3 lookDirection = Vector3.zero;
    //     if (throwMode)
    //     {
    //         currentTargetPosition = throwData.TargetPosition;
    //         lookDirection = throwData.ThrowVelocity;
    //     }

    //     Vector3 aimAt = new Vector3(currentTargetPosition.x, _core.transform.position.y, currentTargetPosition.z);
    //     float distToTarget = Vector3.Distance(aimAt, _barrel.transform.position);
    //     Vector3 relativeTargetPosition = _barrel.transform.position + (_barrel.transform.forward * distToTarget);
    //     relativeTargetPosition = new Vector3(relativeTargetPosition.x, currentTargetPosition.y, relativeTargetPosition.z);

    //     float turningSpeed = autoMode ? Time.deltaTime * _turningSpeed : float.MaxValue;

    //     _barrel.transform.rotation = Quaternion.Slerp(_barrel.transform.rotation, Quaternion.LookRotation(throwMode ? lookDirection : relativeTargetPosition - _barrel.transform.position), turningSpeed);
    //     _core.transform.rotation = Quaternion.Slerp(_core.transform.rotation, Quaternion.LookRotation(aimAt - _core.transform.position), turningSpeed);
    // }

    public void AimDirection(GameObject targetObject)
    {
        // Vector3 aimPosition = ObjectPositionFunction.GetCenterOrPosition(targetObject);

        float turningSpeed = autoMode ? Time.deltaTime * _turningSpeed : float.MaxValue;
        Vector3 targetDirection = new();
        switch (shootMode)
        {
            case ShootMode.Throw:
                ThrowData throwData = GetThrowData(firePoint.position, targetObject);
                targetDirection = throwData.ThrowVelocity;
                break;
            case ShootMode.Direct:
                Vector3 shootData = GetShootData(firePoint.position, targetObject);
                targetDirection = shootData;
                break;
        }

        //turn
        Vector3 projectedToCore = Vector3.ProjectOnPlane(targetDirection, _core.transform.up);
        _core.transform.rotation = Quaternion.Slerp(_core.transform.rotation, Quaternion.LookRotation(projectedToCore),
            turningSpeed);

        //up down
        Vector3 projectedToBarrel = Vector3.ProjectOnPlane(targetDirection, -_barrel.transform.right);
        _barrel.transform.rotation = Quaternion.Slerp(_barrel.transform.rotation,
            Quaternion.LookRotation(projectedToBarrel), turningSpeed);
    }

    public void TriggerShoot(GameObject targetObject)
        => DetectShoot(targetObject, FireAnimate);

    private void FireAnimate()
    {
        if (_hasAnimator)
            _fireAnimate.Play("Shoot", -1, 0f);
    }
}