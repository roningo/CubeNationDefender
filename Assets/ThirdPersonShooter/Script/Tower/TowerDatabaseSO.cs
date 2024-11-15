using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonShooter.Script.Tower
{
    [CreateAssetMenu]
    public class TowerDatabaseSO : ScriptableObject
    {
        public List<ObjectData> objectDatas;
    }

    [Serializable]
    public class ObjectData 
    {
        [field: SerializeField]
        public string Name { get; private set; }
        [field: SerializeField]
        public int ID { get; private set; }
        [field: SerializeField]
        public GameObject Prefab { get; private set; }
    }
}