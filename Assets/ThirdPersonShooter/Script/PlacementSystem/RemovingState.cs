using UnityEngine;

namespace ThirdPersonShooter.Script
{
    public class RemovingState : IBuildingState
    {
        private int _towerIndex = -1;
        private PlacementReview _placementReview;
        private GridData _towerData;
        private TowerPlacer _towerPlacer;

        public RemovingState(
            PlacementReview placementReview,
            GridData towerData,
            TowerPlacer towerPlacer)
        {
            _placementReview = placementReview;
            _towerData = towerData;
            _towerPlacer = towerPlacer;

            _placementReview.StartShowingRemoveReview();
        }

        public void EndState()
        {
            _placementReview.StopShowing();
        }

        public void OnAction(Vector3 placePosition, Vector3Int gridPosition)
        {
            _towerIndex = _towerData.GetResponseIndex(gridPosition);
            if (_towerIndex == -1) return;
            _towerData.RemoveObjectAt(gridPosition);
            _towerPlacer.RemoveObjectAt(_towerIndex);

            _placementReview.UpdatePosition(placePosition, CheckIfSelectionValid(gridPosition));
        }

        private bool CheckIfSelectionValid(Vector3Int gridPosition)
        {
            return !_towerData.CanPlacedObjectAt(gridPosition);
        }

        public void UpdateState(Vector3 placePosition, Vector3Int gridPosition)
        {
            bool validity = CheckIfSelectionValid(gridPosition);
            _placementReview.UpdatePosition(placePosition, validity);
        }
    }
}