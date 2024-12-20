using StarterAssets;
using ThirdPersonShooter.Scripts.Placement;
using ThirdPersonShooter.Scripts.Tower;
using Unity.Cinemachine;
using UnityEngine;

namespace ThirdPersonShooter.Scripts
{
    public class InteractSystem : MonoBehaviour
    {
        [SerializeField] private Transform _interactPoint;
        [SerializeField] private float _interactRadius = 0.7f;
        [SerializeField] private LayerMask _interactableLayer;

        [SerializeField] private InputManager _inputManager;
        [SerializeField] private PlacementSystem _placementSystem;

        [Header("Toggle On Interact")] [SerializeField]
        private CinemachineCamera _towerVirtualCamera;

        [SerializeField] private GameObject _player;

        private Collider[] _interactableList = new Collider[1];
        private int _interactableCount;

        [SerializeField] private Sprite _interactSprite;

        private void OnEnable()
        {
            _inputManager.OnInteract.AddListener(InteractTower);
        }

        private void OnDisable()
        {
            _inputManager.OnInteract.RemoveListener(InteractTower);
        }

        private void Update()
        {
            _interactableCount = Physics.OverlapSphereNonAlloc(_interactPoint.position, _interactRadius,
                _interactableList, _interactableLayer);

            UIManager.interactable = _interactableCount > 0;
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
                _towerVirtualCamera.GetComponent<CinemachineCamera>().LookAt = towerRoot;
                _towerVirtualCamera.GetComponent<CinemachineCamera>().Follow = towerRoot;
                _towerVirtualCamera.gameObject.SetActive(true);

                //self toggle
                _player.GetComponent<ThirdPersonShooterController>().enabled = false;

                //Hit object toggle
                collapseObject.GetComponent<TurretAuto>().autoMode = false;
                collapseObject.GetComponent<TowerController>().enabled = true;
                collapseObject.GetComponent<TowerShooterController>().enabled = true;

                UIManager.SetCurrentActiveIcon(_interactSprite);

                //hide self
                _player.SetActive(false);

                break;
            }
        }
    }
}