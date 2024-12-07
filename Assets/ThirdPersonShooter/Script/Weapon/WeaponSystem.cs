using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

namespace ThirdPersonShooter.Script.Weapon
{
    [DisallowMultipleComponent]
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] private InputManager _inputManager;

        [SerializeField] private WeaponDatabaseSO _weaponDatabase;
        [SerializeField] private Image _currentWeaponImage;
        [SerializeField] private Transform _weaponParent;
        [SerializeField] private int _selectedIndex = 0;

        private bool _triggered = false;

        private GameObject _activeWeapon;
        private GameObject _model;

        // Start is called before the first frame update
        private void Start()
        {
            ConstraintSource constraintSource = new()
            {
                sourceTransform = _weaponParent,
                weight = 1
            };

            if (gameObject.TryGetComponent<ParentConstraint>(out ParentConstraint parentConstraint) &&
                parentConstraint.sourceCount <= 0)
            {
                parentConstraint.AddSource(constraintSource);
            }

            _inputManager.OnScroll.AddListener(OnScroll);
            SelectedWeapon();
        }

        private void OnScroll()
        {
            if (_triggered) return;
            _triggered = true;

            int previousSelectedIndex = _selectedIndex;

            float scroll = _inputManager.starterAssetsInputs.scroll.normalized.y;
            switch (scroll)
            {
                case > 0 when _selectedIndex >= _weaponDatabase.weaponDatas.Count - 1:
                    _selectedIndex = 0;
                    break;
                case > 0:
                    _selectedIndex++;
                    break;
                case < 0 when _selectedIndex <= 0:
                    _selectedIndex = _weaponDatabase.weaponDatas.Count - 1;
                    break;
                case < 0:
                    _selectedIndex--;
                    break;
            }

            if (previousSelectedIndex != _selectedIndex)
                SelectedWeapon();

            Invoke(nameof(ResetTrigger), 0.01f);
        }

        private void ResetTrigger()
        {
            _triggered = false;
        }

        private void SelectedWeapon()
        {
            _activeWeapon = _weaponDatabase.weaponDatas[_selectedIndex].Prefab;
            GameObject weapon = _activeWeapon;
            Spawn(weapon);
            if (_currentWeaponImage)
                _currentWeaponImage.sprite = _weaponDatabase.weaponDatas[_selectedIndex].Icon;
        }

        public void Spawn(GameObject weapon)
        {
            Destroy(_model);
            weapon.GetComponent<ProjectileWeapon>().inputManager = _inputManager;
            _model = Instantiate(weapon, transform, false);
        }
    }
}