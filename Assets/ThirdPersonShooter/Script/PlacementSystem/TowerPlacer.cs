using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonShooter.Script
{
    public class TowerPlacer : MonoBehaviour
    {
        private List<GameObject> _placedTowers = new();

        public int PlaceTower(GameObject towerSetup, Vector3 currentViewPosition)
        {
            GameObject newTower = Instantiate(towerSetup, currentViewPosition, Quaternion.identity);
            _placedTowers.Add(newTower);
            return _placedTowers.Count - 1;
        }

        public void RemoveObjectAt(int towerIndex)
        {
            if (_placedTowers.Count <= towerIndex || !_placedTowers[towerIndex]) return;
            Destroy(_placedTowers[towerIndex]);
            _placedTowers[towerIndex] = null; 
        }
    }
}