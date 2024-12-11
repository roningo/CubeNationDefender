using StarterAssets;
using StarterAssets.InputSystem;
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
        [SerializeField] private TowerDatabaseSO _towerDatabase;

        [SerializeField] private TowerPlacer _towerPlacer;
        private IBuildingState _buildingState;

        [SerializeField] private GameObject _player;

        private GridData _towerPlacementData;
        private Vector3 _currentPlacePosition;
        private Vector3Int _currentGridPosition;
        private PlacementReview _placementReview;
        private Vector3Int _lastDetectedPosition = Vector3Int.zero;

        //Scroll selected
        private int _selectedIndex = 0;
        private bool _triggered = false;
        
        [SerializeField] private Sprite _placementSprite;
        [SerializeField] private Sprite _removingSprite;

        private void Awake()
        {
            starterAssetsInputs = _inputManager.GetComponent<StarterAssetsInputs>();
            _placementReview = GetComponent<PlacementReview>();
        }

        private void Start()
        {
            StopPlacement();
            _towerPlacementData = new GridData();
            UIManager.SetupTowerSlotUI(_towerDatabase.towerDatas);
        }

        void Update()
        {
            if (_buildingState != null && UtilsVariables.CurrentActiveState != UtilsVariables.ActiveState.Placement &&
                UtilsVariables.CurrentActiveState != UtilsVariables.ActiveState.Removing)
            {
                StopPlacement();
            }

            if (starterAssetsInputs.alpha1) StartPlacement(1);
            else if (starterAssetsInputs.alpha2) StartPlacement(2);
            else if (starterAssetsInputs.alpha3) StartPlacement(3);
            else if (starterAssetsInputs.alpha9) StartRemoving();

            starterAssetsInputs.alpha1 = starterAssetsInputs.alpha2 =
                starterAssetsInputs.alpha3 = starterAssetsInputs.alpha9 = false;

            if (_buildingState == null) return;

            PlacementPositionScan();

            if (_lastDetectedPosition == _currentGridPosition) return;

            _buildingState.UpdateState(_currentPlacePosition, _currentGridPosition);
            _lastDetectedPosition = _currentGridPosition;
        }

        private void StartPlacement(int id)
        {
            StopPlacement();
            _selectedIndex = id - 1;
            _buildingState = new PlacementState(id,
                _placementReview,
                _towerDatabase,
                _towerPlacementData,
                _towerPlacer,
                _towerVirtualCamera,
                _player,
                _inputManager,
                starterAssetsInputs);

            PlacementPositionScan();
            _buildingState.UpdateState(_currentPlacePosition, _currentGridPosition);

            _inputManager.OnShoot.AddListener(PlaceTower);
            _inputManager.OnScroll.AddListener(OnScroll);
            _inputManager.OnAim.AddListener(StopPlacement);

            UIManager.UpdateTowerSlotUI(id);

            UtilsVariables.CurrentActiveState = UtilsVariables.ActiveState.Placement;
            UIManager.SetCurrentActiveIcon(_placementSprite);
        }

        private void StartRemoving()
        {
            StopPlacement();
            _buildingState = new RemovingState(
                _placementReview,
                _towerPlacementData,
                _towerPlacer);

            PlacementPositionScan();
            _buildingState.UpdateState(_currentPlacePosition, _currentGridPosition);

            _inputManager.OnShoot.AddListener(PlaceTower);
            _inputManager.OnAim.AddListener(StopPlacement);

            UIManager.UpdateTowerSlotUI();

            UtilsVariables.CurrentActiveState = UtilsVariables.ActiveState.Removing;
            UIManager.SetCurrentActiveIcon(_removingSprite);
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
            _inputManager.OnScroll.RemoveListener(OnScroll);
            _inputManager.OnAim.RemoveListener(StopPlacement);
            
            _lastDetectedPosition = Vector3Int.zero;
            _buildingState = null;

            UtilsVariables.CurrentActiveState = UtilsVariables.ActiveState.Equipment;
            UIManager.UpdateTowerSlotUI();
        }

        private void OnScroll()
        {
            if (_triggered) return;
            _triggered = true;
            
            int previousSelectedIndex = _selectedIndex;

            float scroll = _inputManager.starterAssetsInputs.scroll.normalized.y;
            switch (scroll)
            {
                case > 0 when _selectedIndex >= UIManager.towerSlotUIList.Count - 1:
                    _selectedIndex = 0;
                    break;
                case > 0:
                    _selectedIndex++;
                    break;
                case < 0 when _selectedIndex <= 0:
                    _selectedIndex = UIManager.towerSlotUIList.Count - 1;
                    break;
                case < 0:
                    _selectedIndex--;
                    break;
            }

            if (previousSelectedIndex != _selectedIndex)
                StartPlacement(_selectedIndex + 1);

            Invoke(nameof(ResetTrigger), 0.01f);
        }

        private void ResetTrigger()
        {
            _triggered = false;
        }
    }
}