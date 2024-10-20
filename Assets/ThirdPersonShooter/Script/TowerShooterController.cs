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

    private GatlingAuto _towerShoot;
    private Vector3 _mouseWorldPosition;

    private void Awake()
    {
        _towerController = GetComponent<TowerController>();
        _playerInputs = GetComponent<PlayerInput>();

        _towerShoot = GetComponent<GatlingAuto>();
    }

    void Start()
    {
        _inputManager.OnShoot.AddListener(() => _towerShoot.Shoot(_mouseWorldPosition));
        _inputManager.OnInteract.AddListener(ExitTurret);
    }

    private void Update()
    {
        _mouseWorldPosition = _inputManager.GetMouseWorldPosition();
        _towerShoot.LookAtTarget(_mouseWorldPosition);
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
