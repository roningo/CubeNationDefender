using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _keyText;
    [SerializeField] private Image _icon;

    private int _id;
    private Vector3 _pos;

    private void Start()
    {
        _pos = transform.position;
    }

    public void SetupTowerSlot(string key, Sprite icon, int id)
    {
        if (_keyText) _keyText.text = key;
        if (_icon) _icon.sprite = icon;
        _id = id;
    }

    public void SelectSlot(int id)
    {
        bool selected = id == _id;
        gameObject.GetComponent<Image>().color = selected ? Color.yellow : Color.white;
        transform.Find("KeyBg").GetComponent<Image>().color = selected ? Color.yellow : Color.white;
    }
}