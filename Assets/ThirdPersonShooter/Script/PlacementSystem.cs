using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    private StarterAssetsInputs starterAssetsInputs;

    [Tooltip("Set camera for placement tower")] [SerializeField]
    private CinemachineVirtualCamera _towerVirtualCamera;

    [SerializeField] private GameObject _mouseIndicator;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private Grid _grid;

    [SerializeField] private AudioClip _placeAudio;
    [SerializeField] private TowerDatabaseSO _database;
    private int _selectedTowerIndex = -1;

    private GridData _towerPlacementData;

    private PlacementReview _placementReview;
    private Vector3Int _lastDetectedPosition = Vector3Int.zero;

    private List<GameObject> _placedTower = new();

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
        else if (starterAssetsInputs.alpha4) StartPlacement(3);
        else if (starterAssetsInputs.alpha5) StartPlacement(4);
        else if (starterAssetsInputs.alpha6) StartPlacement(5);
        else if (starterAssetsInputs.alpha7) StartPlacement(6);

        if (_selectedTowerIndex < 0) return;

        Vector3 mousePosition = _inputManager.GetMouseWorldPosition();
        Vector3Int gridPosition = _grid.WorldToCell(mousePosition);
        Vector3 cellPosition = _grid.CellToWorld(gridPosition);
        Vector3 cellIndicatorPosition = new Vector3(cellPosition.x, mousePosition.y, cellPosition.z);


        if (_lastDetectedPosition == gridPosition) return;

        bool placementValidity = CheckPlacementValidity(gridPosition, _selectedTowerIndex);

        _placementReview.UpdatePosition(cellIndicatorPosition, placementValidity);

        _lastDetectedPosition = gridPosition;
    }

    private void StartPlacement(int ID)
    {
        StopPlacement();
        _selectedTowerIndex = _database.objectDatas.FindIndex(data => data.ID == ID);
        if (_selectedTowerIndex < 0)
        {
            Debug.LogError($"ID not found {ID}");
            return;
        }

        _placementReview.StartShowing(_database.objectDatas[_selectedTowerIndex].Prefab);
        _inputManager.OnShoot.AddListener(PlaceTower);
        _inputManager.OnExit.AddListener(StopPlacement);
    }

    private void PlaceTower()
    {
        _inputManager.starterAssetsInputs.shoot = false;

        Vector3 mousePosition = _inputManager.GetMouseWorldPosition();
        Vector3Int gridPosition = _grid.WorldToCell(mousePosition);
        Vector3 cellPosition = _grid.CellToWorld(gridPosition);
        Vector3 placementPosition = new Vector3(cellPosition.x, mousePosition.y, cellPosition.z);

        bool placementValidity = CheckPlacementValidity(gridPosition, _selectedTowerIndex);
        if (!placementValidity) return;

        AudioSource.PlayClipAtPoint(_placeAudio, placementPosition);

        //setup for controllable turret
        GameObject towerSetup = _database.objectDatas[_selectedTowerIndex].Prefab;
        if (towerSetup.TryGetComponent<TowerShooterController>(out TowerShooterController towerShooterController))
        {
            towerShooterController.towerVirtualCamera = _towerVirtualCamera;
            GameObject player = this.gameObject.transform.parent.gameObject;
            towerShooterController.player = player;

            if (towerSetup.TryGetComponent<TowerShooterController>(out TowerShooterController shooterController))
            {
                shooterController._inputManager = _inputManager;
            }
            if (towerSetup.TryGetComponent<TowerController>(out TowerController towerController))
            {
                towerController.input = starterAssetsInputs;
            }
        }

        GameObject newTower = Instantiate(towerSetup, placementPosition, Quaternion.identity);
        _placedTower.Add(newTower);
        GridData selectedData = _towerPlacementData;
        selectedData.AddObjectAt(gridPosition, _database.objectDatas[_selectedTowerIndex].ID, _placedTower.Count - 1);

        _placementReview.UpdatePosition(placementPosition, false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedTowerIndex)
    {
        GridData selectedData = _towerPlacementData;
        return selectedData.CanPlacedObjectAt(gridPosition);
    }

    public void StopPlacement()
    {
        _selectedTowerIndex = -1;
        _placementReview.StopShowing();
        _inputManager.OnShoot.RemoveListener(PlaceTower);
        _inputManager.OnExit.RemoveListener(StopPlacement);
        _lastDetectedPosition = Vector3Int.zero;
    }
}