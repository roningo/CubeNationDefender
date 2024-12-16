using StarterAssets;
using StarterAssets.InputSystem;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ThirdPersonShooter.Scripts
{
    public class ThirdPersonShooterController : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _aimVirtualCamera;
        [SerializeField] private float _normalSensitivity;
        [SerializeField] private float _aimSensitivity;
        [SerializeField] private InputManager _inputManager;
    
        private StarterAssetsInputs _starterAssetsInputs;
        private ThirdPersonController _thirdPersonController;
        private PlayerInput _playerInputs;

        private void Awake()
        {
            _thirdPersonController = GetComponent<ThirdPersonController>();
            _playerInputs = _inputManager.GetComponent<PlayerInput>();
            _starterAssetsInputs = _inputManager.GetComponent<StarterAssetsInputs>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _thirdPersonController.SetRotateOnMove(false);
            _playerInputs.enabled = false; //restart playerInput
        }

        private void Update()
        {
            if (!_playerInputs.enabled)
                _playerInputs.enabled = true; //restart playerInput

            Vector3 mouseWorldPosition = _inputManager.GetMouseWorldPosition();

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

            if (_starterAssetsInputs.aim)
            {
                _aimVirtualCamera.gameObject.SetActive(true);
                _thirdPersonController.SetSensitivity(_aimSensitivity);
            }
            else
            {
                _aimVirtualCamera.gameObject.SetActive(false);
                _thirdPersonController.SetSensitivity(_normalSensitivity);
            }
        }
    }
}
