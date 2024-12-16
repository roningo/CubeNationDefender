using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonShooter.Scripts.Tower
{
    [CreateAssetMenu]
    public class TowerDatabaseSO : ScriptableObject
    {
        public List<TowerData> towerDatas;
    }

    [Serializable]
    public class TowerData 
    {
        [field: SerializeField]
        public string Name { get; private set; }
        [field: SerializeField]
        public int ID { get; private set; }
        [field: SerializeField]
        public GameObject Prefab { get; private set; }
        [field: SerializeField]
        public Sprite Icon { get; private set; }
    }
}