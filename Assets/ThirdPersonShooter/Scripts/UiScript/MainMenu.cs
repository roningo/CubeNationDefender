using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThirdPersonShooter.Scripts.UiScript
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            Time.timeScale = 1f;
            UtilsVariables.IsGamePaused = false;

            SceneManager.LoadSceneAsync("HexaTile");
        }
    
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
