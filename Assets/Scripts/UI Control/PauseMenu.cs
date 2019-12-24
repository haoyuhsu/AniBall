using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    //public GameObject gameSceneUI;
    public SceneFader sceneFader;
    public string menuSceneName = "MainMenu";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);
        //gameSceneUI.SetActive(!gameSceneUI.activeSelf);

        if (pauseMenuUI.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void Retry()
    {
        Toggle();
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        Toggle();
        sceneFader.FadeTo(menuSceneName);
    }
}
