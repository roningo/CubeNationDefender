using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;

public class TowerShooterController : MonoBehaviour
{

    public GameObject player;
    public CinemachineVirtualCamera towerVirtualCamera;
    [SerializeField] private float _normalSensitivity;
    [SerializeField] private float _aimSensitivity;
    [SerializeField] private InputManager _inputManager;

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

    private void Start()
    {
        _inputManager.OnShoot.AddListener(() => _towerShoot.TriggerShoot(_mouseWorldGameObject));
        _inputManager.OnInteract.AddListener(ExitTurret);
    }

    private void Update()
    {
        _mouseWorldGameObject = _inputManager.GetMouseWorldGameObject();
        _towerShoot.AimDirection(_mouseWorldGameObject);
    }

    private void ExitTurret()
    {
        _inputManager.starterAssetsInputs.interact = false;

        _towerShoot.autoMode = true; //automode

        player.SetActive(true);

        towerVirtualCamera.gameObject.SetActive(false);
        _towerController.enabled = false;
        _playerInputs.enabled = false;
        this.enabled = false;
    }
}
