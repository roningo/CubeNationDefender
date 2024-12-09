using System;
using JetBrains.Annotations;
using ThirdPersonShooter.Script;
using ThirdPersonShooter.Script.Placement;
using ThirdPersonShooter.Script.Weapon;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    [SerializeField] private PlacementSystem _placementSystem;
    [SerializeField] private InteractSystem _interactSystem;
    [SerializeField] private WeaponSystem _weaponSystem;
    [SerializeField] private Image _actionImage;
    private Sprite _lastActionSprite;

    public ActiveMode _currentActiveState = ActiveMode.Equipment;

    public enum ActiveMode
    {
        Equipment,
        Placement,
        Removing
    }

    private void OnEnable()
    {
        _placementSystem.StartPlacementEvent.AddListener(SetCurrentActiveIcon);
        _placementSystem.StartRemovingEvent.AddListener(SetCurrentActiveIcon);

        _placementSystem.StartPlacementEvent.AddListener((_) => { WeaponToggle(false); });
        _placementSystem.StartRemovingEvent.AddListener((_) => { WeaponToggle(false); });
        _placementSystem.StopPlacementEvent.AddListener(() => { WeaponToggle(true); });

        _interactSystem.InteractEvent.AddListener(SetCurrentActiveIcon);

        _weaponSystem.SelectedWeaponEvent.AddListener(SetCurrentActiveIcon);
    }

    private void OnDisable()
    {
        _placementSystem.StartPlacementEvent.RemoveAllListeners();
        _placementSystem.StartRemovingEvent.RemoveAllListeners();
        _placementSystem.StopPlacementEvent.RemoveAllListeners();
        _interactSystem.InteractEvent.RemoveAllListeners();
        _weaponSystem.SelectedWeaponEvent.RemoveAllListeners();
    }

    private void WeaponToggle(bool enable)
    {
        // Debug.Log($"WeaponToggle called {enable}");
        _weaponSystem.gameObject.SetActive(enable);
    }

    private void SetCurrentActiveIcon([CanBeNull] Sprite icon)
    {
        // Debug.Log($"SetCurrentActiveIcon called {icon}");
        if (icon && _actionImage)
        {
            _lastActionSprite = _actionImage.sprite;
            _actionImage.sprite = icon;
        }
        else
            _actionImage.sprite = _lastActionSprite;
    }
}