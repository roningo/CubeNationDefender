using ThirdPersonShooter.Scripts.BioStats;
using UnityEngine;

namespace ThirdPersonShooter.Scripts.Enemy
{
    public class SpawnEnemy : MonoBehaviour
    {
        [SerializeField] private Camera _faceCamera;
        [SerializeField] private Canvas _healthBarCanvas;
        [SerializeField] private Transform _homePosition;

        [SerializeField] private GameObject _enemyPf;
        [SerializeField] private float _spawnTime = 1f;

        private void Start()
        {
            InvokeRepeating(nameof(Spawner), 1f, _spawnTime);
        }

        private void Spawner()
        {
            GameObject enemySetup = Instantiate(_enemyPf, transform.position, Quaternion.identity);

            if (enemySetup.TryGetComponent<TargetDetector>(out TargetDetector targetDetector))
                targetDetector.SetupHome(_homePosition.gameObject);

            if (enemySetup.TryGetComponent<Health>(out Health health))
                health.SetupHealthBar(_healthBarCanvas, _faceCamera);
        }
    }
}