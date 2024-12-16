using System.Collections.Generic;
using ThirdPersonShooter.Scripts.Tower;
using UnityEngine;

namespace ThirdPersonShooter.Scripts.Placement
{
    public class TowerPlacer : MonoBehaviour
    {
        private List<GameObject> _placedTowers = new();

        public int PlaceTower(GameObject towerSetup, Vector3 currentViewPosition)
        {
            GameObject newTower = Instantiate(towerSetup, currentViewPosition, Quaternion.identity);
            newTower.GetComponent<TurretAuto>().SetupTowerObstacle();
            
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