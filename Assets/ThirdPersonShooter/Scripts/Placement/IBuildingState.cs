using UnityEngine;

namespace ThirdPersonShooter.Scripts.Placement
{
    public interface IBuildingState
    {
        void EndState();
        void OnAction(Vector3 placePosition, Vector3Int gridPosition);
        void UpdateState(Vector3 placePosition, Vector3Int gridPosition);
    }
}