using UnityEngine;
using UnityEngine.AI;

public class StraightShootFunction
{
    public float fireForce, upwardForce, spread;
    public Transform firePoint;
    public bool useMovementPrediction;

    public void StraightShoot(GameObject currentBullet, GameObject targetObject)
    {
        Vector3 targetPosition = targetObject.transform.position;
        //caculate direction
        Vector3 directionWithoutSpread = targetPosition - firePoint.position;

        if (useMovementPrediction)
        {
            directionWithoutSpread = GetPredictedPositionShootData(directionWithoutSpread, targetObject);
        }

        //caculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);

        //caculate direction
        Vector3 directionWithSpread = directionWithoutSpread.normalized + new Vector3(x, y, z);

        Vector3 bulletVelocity = directionWithSpread.normalized * fireForce + firePoint.up * upwardForce;
        OnShoot(bulletVelocity, currentBullet);
    }

    private Vector3 GetPredictedPositionShootData(Vector3 directShootData, GameObject targetObject)
    {
        Vector3 shootVelocity = directShootData * fireForce;
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

    private void OnShoot(Vector3 bulletVelocity, GameObject currentBullet)
    {
        if (currentBullet.TryGetComponent<Rigidbody>(out Rigidbody rb))
            rb.AddForce(bulletVelocity, ForceMode.Impulse);
        else
            Debug.LogError("Projectile Rigidbody not found");
    }
}

