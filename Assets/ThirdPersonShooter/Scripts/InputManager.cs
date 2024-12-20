using StarterAssets.InputSystem;
using UnityEngine;
using UnityEngine.Events;

namespace ThirdPersonShooter.Scripts
{
    public class InputManager : MonoBehaviour
    {
        [HideInInspector] public StarterAssetsInputs starterAssetsInputs;

        public GameObject mouseIndicator;
        [SerializeField] private LayerMask _aimColliderLayerMask;

        public UnityEvent OnShoot, OnAim, OnScroll, OnInteract, OnPause, OnRemove;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Awake()
        {
            starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        }

        private void Update()
        {
            if (starterAssetsInputs.pause)
            {
                OnPause?.Invoke();
            }

            if (UtilsVariables.IsGamePaused) return;

            if (starterAssetsInputs.shoot)
            {
                OnShoot?.Invoke();
            }

            if (starterAssetsInputs.aim)
            {
                OnAim?.Invoke();
            }

            if (starterAssetsInputs.scroll != Vector2.zero)
            {
                OnScroll?.Invoke();
            }

            if (starterAssetsInputs.interact)
            {
                OnInteract?.Invoke();
            }

            if (starterAssetsInputs.alpha9)
            {
                OnRemove?.Invoke();
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

        private void FindMouseWord()
        {
            Vector2 screenCenterPoint = new(Screen.width / 2f, Screen.height / 2f);
            if (!_camera) return;
            Ray ray = _camera.ScreenPointToRay(screenCenterPoint);

            Vector3 targetPoint = Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _aimColliderLayerMask)
                ? raycastHit.point
                : ray.GetPoint(100);

            if (mouseIndicator)
                mouseIndicator.transform.position = targetPoint;
        }
    }
}