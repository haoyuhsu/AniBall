using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Canvas gameoverCanvas;
    public Text resultText;
    public Score soccerScore;
    DeathCount deathCount;

    void Start()
    {
        gameoverCanvas.enabled = false;     // 初始時把結束Canvas關閉
    }

    public void OpenGameOverCanvas(string TypeOfGame)
    {
        Time.timeScale = 0f;    // 結束時將時間停止

        /* 根據遊戲類型去決定Game Over形式 */
        if (TypeOfGame == "Soccer Game")
        {
            if (soccerScore != null)
                SoccerGameOver();
        }
        if (TypeOfGame == "Survival Game")
        {
            deathCount = FindObjectOfType<DeathCount> ();
            if (deathCount != null)
                SurvivalGameOver();
        }
        gameoverCanvas.enabled = true;    // 把結束Canvas打開
    }

    void SurvivalGameOver()
    {
        string winnerName = deathCount.gameObject.name;
        resultText.text = winnerName + " Win!";
    }

    void SoccerGameOver()
    {
        /* 依照分數決定印出的結果 */
        float team1Score = soccerScore.team1Score;
        float team2Score = soccerScore.team2Score;

        if (team1Score > team2Score)
        {
            resultText.text = "Team 1 Win!";
        }
        else if (team1Score < team2Score)
        {
            resultText.text = "Team 2 Win!";
        }
        else
        {
            resultText.text = "Tied Game";
        }
    }
}
