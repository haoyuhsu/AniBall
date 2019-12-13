using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    /* 按R鍵可以重新載入該Scene */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
