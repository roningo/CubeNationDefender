using System.Collections.Generic;
using UnityEngine;

public class WeaponSystemOld : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    public List<GameObject> weaponList;
    public int selectedIndex = 0;

    private bool _triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        _inputManager.OnScroll.AddListener(OnScroll);
        SelecedWeapon();
    }

    private void OnScroll()
    {
        if (!_triggered)
        {
            _triggered = true;

            int previousSelectedIndex = selectedIndex;

            float scroll = _inputManager.starterAssetsInputs.scroll.normalized.y;
            if (scroll > 0)
            {
                if (selectedIndex >= transform.childCount - 1)
                    selectedIndex = 0;
                else selectedIndex++;
            }
            else if (scroll < 0)
            {
                if (selectedIndex <= 0)
                    selectedIndex = transform.childCount - 1;
                else selectedIndex--;
            }

            if (previousSelectedIndex != selectedIndex)
                SelecedWeapon();

            Invoke(nameof(ResetTrigger), 0.01f);
        }
    }

    private void ResetTrigger()
    {
        _triggered = false;
    }

    private void SelecedWeapon()
    {
        int index = 0;
        foreach (Transform weapon in transform)
        {
            if (index == selectedIndex)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);

            index++;
        }
    }
}
