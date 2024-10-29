using UnityEngine;
using UnityEngine.AI;

public class ThrowShootFunction
{
    public float fireForce;
    public Transform firePoint;
    public float forceRatio = 0;
    public bool useMovementPrediction;

    public void ThrowShoot(GameObject currentBullet, GameObject targetObject)
    {
        ThrowData throwData = CalculateThrowData(
            targetObject.transform.position,
            currentBullet.transform.position
        );

        if (useMovementPrediction)
        {
            throwData = GetPredictedPositionThrowData(throwData, targetObject);
        }

        Vector3 bulletVelocity = throwData.ThrowVelocity;
        OnThrow(bulletVelocity, currentBullet);
    }

    private ThrowData GetPredictedPositionThrowData(ThrowData directThrowData, GameObject targetObject)
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
            fireForce
        );

        return predictiveThrowData;
    }

    private void OnThrow(Vector3 bulletVelocity, GameObject currentBullet)
    {
        if (currentBullet.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.AddForce(bulletVelocity, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Projectile Rigidbody not found");
        }
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
            fireForce
        );
        throwStrength = Mathf.Lerp(throwStrength, fireForce, forceRatio);

        float angle;
        if (forceRatio == 0)
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
        {
            // you will need to handle this case when there
            // is no feasible angle to throw the object and reach the target.
            return new ThrowData();
        }

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

    private struct ThrowData
    {
        public Vector3 ThrowVelocity;
        public float Angle;
        public float DeltaXZ;
        public float DeltaY;
    }
}

