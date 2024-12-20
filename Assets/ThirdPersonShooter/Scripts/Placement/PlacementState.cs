﻿using System;
using StarterAssets;
using StarterAssets.InputSystem;
using ThirdPersonShooter.Scripts.Tower;
using Unity.Cinemachine;
using UnityEngine;

namespace ThirdPersonShooter.Scripts.Placement
{
    public class PlacementState : IBuildingState
    {
        private int _selectedTowerIndex = -1;
        private int _id;
        private PlacementReview _placementReview;
        private TowerDatabaseSO _towerDatabase;
        private GridData _towerData;
        private TowerPlacer _towerPlacer;

        private CinemachineCamera _towerVirtualCamera;
        private GameObject _player;
        private InputManager _inputManager;
        private StarterAssetsInputs _starterAssetsInputs;

        public PlacementState(int id,
            PlacementReview placementReview,
            TowerDatabaseSO towerDatabase,
            GridData towerData,
            TowerPlacer towerPlacer,
            CinemachineCamera towerVirtualCamera,
            GameObject player,
            InputManager inputManager,
            StarterAssetsInputs starterAssetsInputs)
        {
            _id = id;
            _placementReview = placementReview;
            _towerDatabase = towerDatabase;
            _towerData = towerData;
            _towerPlacer = towerPlacer;
            _towerVirtualCamera = towerVirtualCamera;
            _player = player;
            _inputManager = inputManager;
            _starterAssetsInputs = starterAssetsInputs;

            _selectedTowerIndex = _towerDatabase.towerDatas.FindIndex(data => data.ID == id);
            if (_selectedTowerIndex > -1)
                _placementReview.StartShowingPlacementReview(_towerDatabase.towerDatas[_selectedTowerIndex].Prefab);
            else
                throw new Exception($"Placement ID not found {_id}");
        }

        public void EndState()
        {
            _placementReview.StopShowing();
        }

        public void OnAction(Vector3 placePosition, Vector3Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition);
            if (!placementValidity) return;

            // AudioSource.PlayClipAtPoint(_placeAudio, _currentViewPosition);

            //setup for controllable turret
            GameObject towerSetup = _towerDatabase.towerDatas[_selectedTowerIndex].Prefab;
            if (towerSetup.TryGetComponent<TowerShooterController>(out TowerShooterController towerShooterController))
            {
                towerShooterController.player = _player;
                towerShooterController.towerVirtualCamera = _towerVirtualCamera;
                towerShooterController._inputManager = _inputManager;

                if (towerSetup.TryGetComponent<TowerController>(out TowerController towerController))
                    towerController.input = _starterAssetsInputs;
            }

            int index = _towerPlacer.PlaceTower(towerSetup, placePosition);

            GridData selectedData = _towerData;
            selectedData.AddObjectAt(gridPosition, _towerDatabase.towerDatas[_selectedTowerIndex].ID,
                index);

            PlacementPositionUpdate(placePosition, gridPosition);
        }

        public bool CheckPlacementValidity(Vector3Int gridPosition)
        {
            GridData selectedData = _towerData;
            return selectedData.CanPlacedObjectAt(gridPosition);
        }

        public void PlacementPositionUpdate(Vector3 placePosition, Vector3Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition);
            _placementReview.UpdatePosition(placePosition, placementValidity);
        }

        public void UpdateState(Vector3 placePosition, Vector3Int gridPosition)
        {
            PlacementPositionUpdate(placePosition, gridPosition);
        }
    }
}