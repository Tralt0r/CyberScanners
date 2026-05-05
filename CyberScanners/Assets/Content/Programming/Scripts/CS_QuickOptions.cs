using UnityEngine;
using UnityEngine.SceneManagement;

public class CS_QuickOptions : MonoBehaviour
{
    public bool godMode = false;
    public bool doubleSpeed = false;
    public bool pauseGame = false;

    public void ToggleGodMode()
    {
        godMode = !godMode;
        Debug.Log($"God Mode: {(godMode ? "ON" : "OFF")}");
    }

    public void doubleSpeedToggle()
    {
        doubleSpeed = !doubleSpeed;
        Time.timeScale = doubleSpeed ? 2f : 1f;
        Debug.Log($"Double Speed: {(doubleSpeed ? "ON" : "OFF")}");
    }

    public void TogglePause()
    {
        pauseGame = !pauseGame;
        Time.timeScale = pauseGame ? 0f : 1f;
        Debug.Log($"Pause Game: {(pauseGame ? "ON" : "OFF")}");
    }

    public void leaveGame()
    {
        Debug.Log("Leaving Game...");
        SceneManager.LoadScene("MainMenu");
    }
}
