using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractSystem : MonoBehaviour
{
    [SerializeField] private Transform _interactPoint;
    [SerializeField] private float _interactRadius = 0.7f;
    [SerializeField] private LayerMask _interactableLayer;

    [SerializeField] private InputManager _inputManager;
    [SerializeField] private PlacementSystem _placementSystem;

    [Header("Toggle On Interact")]
    [SerializeField] private CinemachineVirtualCamera _towerVirtualCamera;
    [SerializeField] private GameObject _playerRoot;
    private PlayerInput _playerInputs;

    private Collider[] _interactableList = new Collider[3];
    private int _interactableCount;

    private void Awake()
    {
        _playerInputs = GetComponentInParent<PlayerInput>();
    }

    private void Start()
    {
        _inputManager.OnInteract.AddListener(InteractTower);
    }

    private void Update()
    {
        _interactableCount = Physics.OverlapSphereNonAlloc(_interactPoint.position, _interactRadius, _interactableList, _interactableLayer);
    }

    private void InteractTower()
    {
        _inputManager.starterAssetsInputs.interact = false;

        if (_interactableCount <= 0) return;
        
        foreach (Collider collapseObject in _interactableList)
        {
            if (collapseObject == null || !collapseObject.gameObject.CompareTag("Tower")) continue;
            
            _placementSystem.GetComponent<PlacementSystem>().StopPlacement();

            //camera toggle
            Transform towerRoot = collapseObject.transform.Find("TowerCameraRoot");
            _towerVirtualCamera.GetComponent<CinemachineVirtualCamera>().LookAt = towerRoot;
            _towerVirtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = towerRoot;
            _towerVirtualCamera.gameObject.SetActive(true);
            // _towerVirtualCamera.transform.forward =_playerRoot.transform.forward;

            //self toggle
            _playerInputs.enabled = false;
            GetComponentInParent<ThirdPersonShooterController>().enabled = false;

            //Hit object toggle
            collapseObject.GetComponent<TurretAuto>().autoMode = false;
            collapseObject.GetComponent<TowerController>().enabled = true;
            collapseObject.GetComponent<PlayerInput>().enabled = true;
            collapseObject.GetComponent<TowerShooterController>().enabled = true;

            this.gameObject.transform.parent.gameObject.SetActive(false); //hide self

            break;
        }
    }
}
