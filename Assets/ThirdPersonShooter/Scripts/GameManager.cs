using StarterAssets.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThirdPersonShooter.Scripts
{
    public class GameManager : MonoBehaviour
    {
        private StarterAssetsInputs starterAssetsInputs;

        [SerializeField] private InputManager _inputManager;

        private void Awake()
        {
            starterAssetsInputs = _inputManager.GetComponent<StarterAssetsInputs>();
        }

        private void OnEnable()
        {
            _inputManager.OnPause.AddListener(OnReturn);
        }

        private void OnDisable()
        {
            _inputManager.OnPause.RemoveListener(OnReturn);
        }

        private void OnReturn()
        {
            starterAssetsInputs.pause = false;
            if (UtilsVariables.CurrentActiveState == UtilsVariables.ActiveState.Placement ||
                UtilsVariables.CurrentActiveState == UtilsVariables.ActiveState.Removing)
            {
                UtilsVariables.CurrentActiveState = UtilsVariables.ActiveState.Equipment;
            }
            else
            {
                if (UtilsVariables.IsGamePaused)
                    GameResume();
                else
                    GamePause();
            }
        }

        public void GamePause()
        {
            UtilsVariables.IsGamePaused = true;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;
        }

        public void GameResume()
        {
            UtilsVariables.IsGamePaused = false;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void ReturnToMainMenu()
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}