using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using ThirdPersonShooter.Script.Tower;
using UnityEngine;
using UnityEngine.UI;

namespace ThirdPersonShooter.Script
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private Image _actionImage;
        [SerializeField] private GameObject _uiTowerList;
        [SerializeField] private GameObject _uiTowerSlot;
        [SerializeField] private GameObject _uiInteract;
        [SerializeField] private GameObject _uiCancel;

        private static Image _actionImageChange;
        private static GameObject _uiTowerListChange;
        private static GameObject _uiTowerSlotChange;
        private static GameObject _uiInteractChange;

        private static Sprite _lastActionSprite;

        public static readonly List<GameObject> TowerSlotUIList = new();
        public static bool interactable = false;

        private void Awake()
        {
            _actionImageChange = _actionImage;
            _uiTowerListChange = _uiTowerList;
            _uiTowerSlotChange = _uiTowerSlot;
            _uiInteractChange = _uiInteract;
        }

        private void Update()
        {
            TogglePauseUI();
            ToggleInteractUI();
            ToggleCancelUI();
        }

        public static void SetCurrentActiveIcon([CanBeNull] Sprite icon)
        {
            if (icon && _actionImageChange)
            {
                _lastActionSprite = _actionImageChange.sprite;
                _actionImageChange.sprite = icon;
            }
            else
                _actionImageChange.sprite = _lastActionSprite;
        }

        public static void SetupTowerSlotUI(List<TowerData> towerDatas)
        {
            TowerSlotUIList.Clear();
            int i = 1;
            foreach (TowerData tower in towerDatas)
            {
                GameObject towerSlotUI = Instantiate(_uiTowerSlotChange, _uiTowerListChange.transform);
                towerSlotUI.GetComponentInChildren<TowerSlot>().SetupTowerSlot(i.ToString(), tower.Icon, i);
                TowerSlotUIList.Add(towerSlotUI);
                i++;
            }
        }

        public static void UpdateTowerSlotUI(int id = 0)
        {
            foreach (GameObject towerUI in TowerSlotUIList)
            {
                towerUI.GetComponent<TowerSlot>().SelectSlot(id);
            }
        }

        private void TogglePauseUI()
        {
            if (_pauseMenu.activeSelf == UtilsVariables.IsGamePaused) return;
            _pauseMenu.SetActive(UtilsVariables.IsGamePaused);
        }

        private void ToggleInteractUI()
        {
            if (_uiInteractChange.activeSelf == interactable) return;
            _uiInteractChange.SetActive(interactable);
        }

        private void ToggleCancelUI()
        {
            bool canCancel = UtilsVariables.CurrentActiveState == UtilsVariables.ActiveState.Placement ||
                             UtilsVariables.CurrentActiveState == UtilsVariables.ActiveState.Removing;
            if (_uiCancel.activeSelf == canCancel) return;
            _uiCancel.SetActive(canCancel);
        }
    }
}