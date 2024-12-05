using CrashKonijn.Goap.Behaviours;
using ThirdPersonShooter.Script.BioStats;
using ThirdPersonShooter.Script.GOAP.Behaviors;
using UnityEngine;

namespace ThirdPersonShooter.Script.Enemy
{
    public class SpawnEnemy : MonoBehaviour
    {
        [SerializeField] private Camera _faceCamera;
        [SerializeField] private Canvas _healthBarCanvas;
        [SerializeField] private Transform _homePosition;
        [SerializeField] private GoapRunnerBehaviour _goapRunner;

        [SerializeField] private GameObject _enemyPf;
        [SerializeField] private float _spawnTime = 1f;

        private void Start()
        {
            InvokeRepeating(nameof(Spawner), 1f, _spawnTime);
        }

        private void Spawner()
        {
            GameObject enemySetup = Instantiate(_enemyPf, transform.position, Quaternion.identity);

            if (enemySetup.TryGetComponent<FindDestination>(out FindDestination findDestination))
                findDestination.homeDestination = _homePosition;

            if (enemySetup.TryGetComponent<Health>(out Health health))
                health.SetupHealthBar(_healthBarCanvas, _faceCamera);

            if (enemySetup.TryGetComponent<GoapSetBinder>(out GoapSetBinder goapSetBinder))
                goapSetBinder.Setup(_goapRunner);
        }
    }
}