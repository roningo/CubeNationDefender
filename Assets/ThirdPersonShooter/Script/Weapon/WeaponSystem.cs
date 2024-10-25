using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[DisallowMultipleComponent]
public class WeaponSystem : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;

    [SerializeField] private List<GameObject> _weaponList;
    [SerializeField] private Transform _weaponParent;
    [SerializeField] private int _selectedIndex = 0;

    private bool _triggered = false;
    
    private GameObject _activeWeapon;
    private GameObject _model;

    // Start is called before the first frame update
    void Start()
    {
        ConstraintSource constraintSource = new()
        {
            sourceTransform = _weaponParent,
            weight = 1
        };

        if (gameObject.TryGetComponent<ParentConstraint>(out ParentConstraint parentConstraint) && parentConstraint.sourceCount <= 0)
        {
            parentConstraint.AddSource(constraintSource);
        }
        _inputManager.OnScroll.AddListener(OnScroll);
        SelectedWeapon();
    }

    private void OnScroll()
    {
        if (!_triggered)
        {
            _triggered = true;

            int previousSelectedIndex = _selectedIndex;

            float scroll = _inputManager.starterAssetsInputs.scroll.normalized.y;
            if (scroll > 0)
            {
                if (_selectedIndex >= _weaponList.Count - 1)
                    _selectedIndex = 0;
                else _selectedIndex++;
            }
            else if (scroll < 0)
            {
                if (_selectedIndex <= 0)
                    _selectedIndex = _weaponList.Count - 1;
                else _selectedIndex--;
            }

            if (previousSelectedIndex != _selectedIndex)
                SelectedWeapon();

            Invoke(nameof(ResetTrigger), 0.01f);
        }
    }

    private void ResetTrigger()
    {
        _triggered = false;
    }

    private void SelectedWeapon()
    {
        _activeWeapon = _weaponList[_selectedIndex];
        GameObject weapon = _activeWeapon;
        Spawn(weapon);
    }

    public void Spawn(GameObject weapon)
    {
        Destroy(_model);
        weapon.GetComponent<ProjectileWeapon>().inputManager = _inputManager;
        _model = Instantiate(weapon);
        _model.transform.SetParent(transform, false);
    }
}
