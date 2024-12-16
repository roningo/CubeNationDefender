using StarterAssets;
using ThirdPersonShooter.Scripts.Tower;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ThirdPersonShooter.Scripts
{
    public class TowerShooterController : MonoBehaviour
    {

        public GameObject player;
        public CinemachineCamera towerVirtualCamera;
        [SerializeField] private float _normalSensitivity;
        [SerializeField] private float _aimSensitivity;
        public InputManager _inputManager;

        private TowerController _towerController;
        private PlayerInput _playerInputs;

        private TurretAuto _towerShoot;
        private GameObject _mouseWorldGameObject;

        private void Awake()
        {
            _towerController = GetComponent<TowerController>();
            _playerInputs = GetComponent<PlayerInput>();

            _towerShoot = GetComponent<TurretAuto>();
        }

        private void OnEnable()
        {
            _inputManager.OnShoot.AddListener(ListenShoot);
            _inputManager.OnInteract.AddListener(ExitTurret);
        }

        private void OnDisable()
        {
            _inputManager.OnShoot.RemoveListener(ListenShoot);
            _inputManager.OnInteract.RemoveListener(ExitTurret);
        }

        private void Update()
        {
            _mouseWorldGameObject = _inputManager.GetMouseWorldGameObject();
            _towerShoot.AimDirection(_mouseWorldGameObject);
        }

        private void ListenShoot() => _towerShoot.TriggerShoot(_mouseWorldGameObject);

        private void ExitTurret()
        {
            _inputManager.starterAssetsInputs.interact = false;

            _towerShoot.autoMode = true; //automode

            player.SetActive(true);

            towerVirtualCamera.gameObject.SetActive(false);
            _towerController.enabled = false;
            enabled = false;
        }
    }
}
