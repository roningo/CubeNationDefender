using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    [HideInInspector] public StarterAssetsInputs starterAssetsInputs;

    public GameObject mouseIndicator;
    [SerializeField] private LayerMask _aimColliderLayerMask = new LayerMask();

    public UnityEvent OnShoot, OnScroll, OnInteract, OnExit;

    private void Awake()
    {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (starterAssetsInputs.shoot)
        {
            OnShoot?.Invoke();
        }
        if (starterAssetsInputs.scroll != Vector2.zero)
        {
            OnScroll?.Invoke();
        }
        if (starterAssetsInputs.interact)
        {
            OnInteract?.Invoke();
        }
        if (starterAssetsInputs.alpha0)
        {
            OnExit?.Invoke();
        }
    }

    public Vector3 GetMouseWorldPosition()
    {
        FindMouseWord();
        return mouseIndicator.transform.position;
    }

    public GameObject GetMouseWorldGameObject()
    {
        FindMouseWord();
        return mouseIndicator;
    }

    private void FindMouseWord() {
        Vector2 screenCenterPoint = new(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _aimColliderLayerMask))
            targetPoint = raycastHit.point;
        else
            targetPoint = ray.GetPoint(100);

        if (mouseIndicator != null)
            mouseIndicator.transform.position = targetPoint;
    }

    // public Vector3 GetGunPointPosition()
    // {

    //     Vector3 GunPointPosition = Vector3.zero;
    //     Vector2 screenCenterPoint = new(Screen.width / 2f, Screen.height / 2f);
    //     Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

    //     Vector3 targetPoint = ray.GetPoint(10);

    //     if (mouseIndicator != null)
    //         mouseIndicator.transform.position = targetPoint;
    //     GunPointPosition = targetPoint;
    //     return GunPointPosition;
    // }
}
