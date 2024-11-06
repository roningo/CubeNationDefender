using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float destroyTime = 1f;
    public Vector3 offset = new Vector3(0f, 1.5f, 0f);
    public Vector3 randomPosition = Vector3.zero;

    private void Start()
    {
        Destroy(gameObject, destroyTime);

        transform.localPosition += offset;
        float randomX = Random.Range(-randomPosition.x, randomPosition.x);
        float randomY = Random.Range(-randomPosition.y, randomPosition.y);
        float randomZ = Random.Range(-randomPosition.z, randomPosition.z);
        transform.localPosition += new Vector3(randomX, randomY, randomZ);
    }
}
 