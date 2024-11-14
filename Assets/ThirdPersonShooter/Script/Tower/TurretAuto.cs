using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class TurretAuto : ProjectileWeapon
{
    [Header("Turret stats")] public CustomTrigger attackRageTrigger;

    [SerializeField] private GameObject _core, _barrel;
    [SerializeField] private float _turningSpeed = 20f;

    [SerializeField] private float _fireRange = 7f;

    [FormerlySerializedAs("enemyLayer")] [SerializeField]
    private LayerMask _enemyLayer;

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

        GameObject rangeObject = attackRageTrigger.gameObject;
        if (rangeObject && rangeObject.TryGetComponent<SphereCollider>(out SphereCollider sphereResult))
            sphereResult.radius = _fireRange;
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

    public void AimDirection(GameObject targetObject)
    {
        float turningSpeed = autoMode ? Time.deltaTime * _turningSpeed : float.MaxValue;
        Vector3 targetDirection = new();
        switch (shootMode)
        {
            case ShootMode.Throw:
                ThrowData throwData = GetThrowData(firePoint.position, targetObject);
                targetDirection = throwData.ThrowVelocity;
                break;
            case ShootMode.Direct:
                Vector3 shootData = GetShootData(_barrel.transform.position, targetObject);
                targetDirection = shootData;
                break;
        }

        //turn
        Vector3 projectedToCore = Vector3.ProjectOnPlane(targetDirection, _core.transform.up);
        _core.transform.rotation = Quaternion.Slerp(
            _core.transform.rotation,
            Quaternion.LookRotation(projectedToCore, _core.transform.up),
            turningSpeed);

        //up down
        Vector3 projectedToBarrel = Vector3.ProjectOnPlane(targetDirection, -_barrel.transform.right);
        _barrel.transform.rotation = Quaternion.Slerp(
            _barrel.transform.rotation,
            Quaternion.LookRotation(projectedToBarrel),
            turningSpeed);
        //need better solution here
        //for locking barrel not false rotate y angle
        _barrel.transform.localEulerAngles =
            new Vector3(_barrel.transform.localEulerAngles.x, 0, _barrel.transform.localEulerAngles.z);
    }

    private bool CheckRaycastToEnemy()
    {
        bool isPointEnemy = false;
        if (Physics.Raycast(_barrel.transform.position,
                _barrel.transform.forward,
                out RaycastHit hit,
                _fireRange,
                _enemyLayer))
        {
            hit.transform.CompareTag("Enemy");
            isPointEnemy = true;
        }

        return isPointEnemy;
    }

    public void TriggerShoot(GameObject targetObject)
        => DetectShoot(targetObject, FireAnimate);

    private void FireAnimate()
    {
        if (_hasAnimator)
            _fireAnimate.Play("Shoot", -1, 0f);
    }
}