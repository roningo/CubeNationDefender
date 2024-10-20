using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private int _selectedIndex = 0;

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

            int previousSelectedIndex = _selectedIndex;

            float scroll = _inputManager.starterAssetsInputs.scroll.normalized.y;
            if (scroll > 0)
            {
                if (_selectedIndex >= transform.childCount - 1)
                    _selectedIndex = 0;
                else _selectedIndex++;
            }
            else if (scroll < 0)
            {
                if (_selectedIndex <= 0)
                    _selectedIndex = transform.childCount - 1;
                else _selectedIndex--;
            }

            if (previousSelectedIndex != _selectedIndex)
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
            if (index == _selectedIndex)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);

            index++;
        }
    }
}
