using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private Camera _faceCamera;
    [SerializeField] private Canvas _healthBarCanvas;

    [SerializeField] private GameObject _enemyPf;
    [SerializeField] private Transform _homePosition;
    [SerializeField] private float _spawnTime = 1f;
    
    void Start()
    {
        InvokeRepeating(nameof(Spawner), 1f, _spawnTime);
    }

    void Spawner()
    {
        GameObject enemy = Instantiate(_enemyPf, transform.position, Quaternion.identity);

        if (enemy.TryGetComponent<FindDestination>(out FindDestination findDestination))
            findDestination.homeDestination = _homePosition;

        if (enemy.TryGetComponent<Health>(out Health health))
            health.SetupHealthBar(_healthBarCanvas, _faceCamera);
    }
}
