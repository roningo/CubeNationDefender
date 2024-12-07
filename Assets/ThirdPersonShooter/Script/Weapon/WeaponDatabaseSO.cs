using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ThirdPersonShooter.Script.Weapon
{
    [CreateAssetMenu]
    public class WeaponDatabaseSO : ScriptableObject
    {
        public List<WeaponData> weaponDatas;
    }

    [Serializable]
    public class WeaponData
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