using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    //public GameObject gameSceneUI;
    public SceneFader sceneFader;
    public string menuSceneName = "MainMenu";
    public Sprite PlaySprite;
    public Sprite PauseSprite;
    public Image ButtonImage;

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

        if (pauseMenuUI.activeSelf)
        {
            ButtonImage.sprite = PauseSprite;
            Time.timeScale = 0f;
        }
        else
        {
            ButtonImage.sprite = PlaySprite;
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
