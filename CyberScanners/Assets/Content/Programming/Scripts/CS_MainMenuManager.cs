using UnityEngine;
using UnityEngine.SceneManagement;

public class CS_MainMenuManager : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    } 

    public void StartGame()
    {
        int nextBuildIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextBuildIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextBuildIndex);
        }
    }
}
