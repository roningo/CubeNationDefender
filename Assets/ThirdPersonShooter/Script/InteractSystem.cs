using Cinemachine;
using StarterAssets;
using ThirdPersonShooter.Script.Tower;
using UnityEngine;

namespace ThirdPersonShooter.Script
{
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

        private Collider[] _interactableList = new Collider[1];
        private int _interactableCount;

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
                GetComponentInParent<ThirdPersonShooterController>().enabled = false;

                //Hit object toggle
                collapseObject.GetComponent<TurretAuto>().autoMode = false;
                collapseObject.GetComponent<TowerController>().enabled = true;
                collapseObject.GetComponent<TowerShooterController>().enabled = true;

                gameObject.transform.parent.gameObject.SetActive(false); //hide self

                break;
            }
        }
    }
}
