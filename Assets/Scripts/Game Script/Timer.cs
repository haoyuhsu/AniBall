using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float startTime;                  // 起始倒數時間
    float curTime;                    // 目前剩餘時間
    bool gameEnd;                     // 是否結束遊戲
    public Text timeText;
    public string TypeOfGame;
    GameOver gameOverObj;

    void Start()
    {
        startTime = FindObjectOfType<GameSetting> ().roundTime;
        curTime = startTime;
        gameEnd = false;
        gameOverObj = GetComponent<GameOver> ();
    }

    void FixedUpdate()
    {
        if (curTime <= 0)                         // 時間倒數至0時, 觸發Game Over
        {
            if (!gameEnd) gameOver();
            return;
        }
        curTime -= Time.deltaTime;                // 倒數時間
        timeText.text = curTime.ToString("0");    // 時間取整數顯示
    }

    void gameOver()
    {
        gameEnd = true;
        gameOverObj.OpenGameOverCanvas(TypeOfGame);
    }
}
