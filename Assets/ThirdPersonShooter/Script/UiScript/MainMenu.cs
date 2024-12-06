using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("HexaTile");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
