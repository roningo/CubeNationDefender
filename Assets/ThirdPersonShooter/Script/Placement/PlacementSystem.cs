using StarterAssets;
using ThirdPersonShooter.Script.Tower;
using Unity.Cinemachine;
using UnityEngine;

namespace ThirdPersonShooter.Script.Placement
{
    [RequireComponent(typeof(PlacementReview))]
    public class PlacementSystem : MonoBehaviour
    {
        private StarterAssetsInputs starterAssetsInputs;

        [Tooltip("Set camera for placement tower")] [SerializeField]
        private CinemachineVirtualCamera _towerVirtualCamera;

        [SerializeField] private float _maxHeightPlaceable;

        [SerializeField] private LayerMask _placeLayerMask;

        [SerializeField] private GameObject _mouseIndicator;
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private Grid _grid;

        [SerializeField] private AudioClip _placeAudio;
        [SerializeField] private TowerDatabaseSO _database;

        [SerializeField] private TowerPlacer _towerPlacer;
        private IBuildingState _buildingState;

        private GridData _towerPlacementData;
        private Vector3 _currentPlacePosition;
        private Vector3Int _currentGridPosition;
        private PlacementReview _placementReview;
        private Vector3Int _lastDetectedPosition = Vector3Int.zero;

        private void Awake()
        {
            starterAssetsInputs = _inputManager.GetComponent<StarterAssetsInputs>();
            _placementReview = GetComponent<PlacementReview>();
        }

        private void Start()
        {
            StopPlacement();
            _towerPlacementData = new GridData();
        }

        // Update is called once per frame
        void Update()
        {
            if (starterAssetsInputs.alpha1) StartPlacement(0);
            else if (starterAssetsInputs.alpha2) StartPlacement(1);
            else if (starterAssetsInputs.alpha3) StartPlacement(2);
            else if (starterAssetsInputs.alpha9) StartRemoving();
            if (_buildingState == null) return;

            PlacementPositionScan();

            if (_lastDetectedPosition == _currentGridPosition) return;

            _buildingState.UpdateState(_currentPlacePosition, _currentGridPosition);
            _lastDetectedPosition = _currentGridPosition;
        }

        private void StartPlacement(int ID)
        {
            StopPlacement();
            _buildingState = new PlacementState(ID,
                _placementReview,
                _database,
                _towerPlacementData,
                _towerPlacer,
                _towerVirtualCamera,
                gameObject,
                _inputManager,
                starterAssetsInputs);

            PlacementPositionScan();
            _buildingState.UpdateState(_currentPlacePosition, _currentGridPosition);

            _inputManager.OnShoot.AddListener(PlaceTower);
            _inputManager.OnExit.AddListener(StopPlacement);
        }

        private void StartRemoving()
        {
            StopPlacement();
            _buildingState = new RemovingState(
                _placementReview,
                _towerPlacementData,
                _towerPlacer);
            
            _inputManager.OnShoot.AddListener(PlaceTower);
            _inputManager.OnExit.AddListener(StopPlacement);
        }

        private void PlacementPositionScan()
        {
            Vector3 mousePosition = _inputManager.GetMouseWorldPosition();
            Vector3Int gridPosition = _grid.WorldToCell(mousePosition);
            Vector3 cellPosition = _grid.CellToWorld(gridPosition);
            Vector3 currentViewPosition = new Vector3(cellPosition.x, mousePosition.y, cellPosition.z);

            // check if player are looking at wall
            // down ray to the floor in range _maxHeightPlaceable
            Ray ray = new Ray(currentViewPosition + new Vector3(0, 0.01f, 0), Vector3.down);
            Vector3 placePosition =
                Physics.Raycast(ray, out RaycastHit raycastHit, _maxHeightPlaceable, _placeLayerMask)
                    ? raycastHit.point
                    : Vector3.zero;

            if (placePosition != Vector3.zero)
            {
                _currentPlacePosition = placePosition;
                _currentGridPosition = gridPosition;
            }
            else
            {
                _currentPlacePosition = Vector3.zero;
                _currentGridPosition = Vector3Int.zero;
            }
        }

        private void PlaceTower()
        {
            _inputManager.starterAssetsInputs.shoot = false;

            if (_currentPlacePosition == Vector3.zero) return;

            _buildingState.OnAction(_currentPlacePosition, _currentGridPosition);
        }

        public void StopPlacement()
        {
            if (_buildingState == null) return;

            _buildingState.EndState();
            _inputManager.OnShoot.RemoveListener(PlaceTower);
            _inputManager.OnExit.RemoveListener(StopPlacement);
            _lastDetectedPosition = Vector3Int.zero;
            _buildingState = null;
        }
    }
}