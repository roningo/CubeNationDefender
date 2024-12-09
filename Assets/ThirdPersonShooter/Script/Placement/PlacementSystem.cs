using System.Collections.Generic;
using StarterAssets;
using ThirdPersonShooter.Script.Tower;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

        [SerializeField] private GameObject _uiTowerList;
        [SerializeField] private GameObject _uiTowerSlot;
        [SerializeField] private Sprite _placementSprite;
        [SerializeField] private Sprite _removingSprite;
        public UnityEvent<Sprite> StartPlacementEvent;
        public UnityEvent<Sprite> StartRemovingEvent;
        public UnityEvent StopPlacementEvent;
        
        private List<GameObject> _towerUIList = new();

        private void Awake()
        {
            starterAssetsInputs = _inputManager.GetComponent<StarterAssetsInputs>();
            _placementReview = GetComponent<PlacementReview>();
        }

        private void Start()
        {
            StopPlacement();
            _towerPlacementData = new GridData();
            SetTowerListUI();
        }

        // Update is called once per frame
        void Update()
        {
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

        private void SetTowerListUI()
        {
            _towerUIList.Clear();
            int i = 1;
            foreach (TowerData tower in _towerDatabase.towerDatas)
            {
                GameObject towerSlotUI = Instantiate(_uiTowerSlot, _uiTowerList.transform);
                towerSlotUI.GetComponentInChildren<TowerSlot>().SetupTowerSlot(i.ToString(), tower.Icon, i);
                _towerUIList.Add(towerSlotUI);
                i++;
            }
        }

        private void UpdateTowerUI(int id = 0)
        {
            foreach (GameObject towerUI in _towerUIList)
            {
                towerUI.GetComponent<TowerSlot>().SelectSlot(id);
            }
        } 

        private void StartPlacement(int id)
        {
            StopPlacement();
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
            _inputManager.OnExit.AddListener(StopPlacement);

            UpdateTowerUI(id);

            StartPlacementEvent?.Invoke(_placementSprite);
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
            _inputManager.OnExit.AddListener(StopPlacement);
            
            UpdateTowerUI();

            StartRemovingEvent?.Invoke(_removingSprite);
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
            
            UpdateTowerUI();

            StopPlacementEvent?.Invoke();
        }
    }
}