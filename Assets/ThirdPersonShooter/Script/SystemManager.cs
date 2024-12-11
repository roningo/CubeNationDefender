using ThirdPersonShooter.Script;
using ThirdPersonShooter.Script.Placement;
using ThirdPersonShooter.Script.Weapon;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    [SerializeField] private PlacementSystem _placementSystem;
    [SerializeField] private InteractSystem _interactSystem;
    [SerializeField] private WeaponSystem _weaponSystem;

    private void Update()
    {
        ToggleWeaponSystem();
    }
    
    private void ToggleWeaponSystem()
    {
        bool isWeapon = UtilsVariables.CurrentActiveState == UtilsVariables.ActiveState.Equipment;
        if (_weaponSystem.gameObject.activeSelf == isWeapon) return;
        _weaponSystem.gameObject.SetActive(isWeapon);
    }
}