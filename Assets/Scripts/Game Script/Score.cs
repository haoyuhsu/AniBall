using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int team1Score = 0;
    public int team2Score = 0;
    public Text team1ScoreText;
    public Text team2ScoreText;
    int maxScore;

    void Start()
    {
        maxScore = FindObjectOfType<GameSetting> ().maxScore;
        team1ScoreText.text = "Team 1 Score : " + team1Score;
        team2ScoreText.text = "Team 2 Score : " + team2Score;
    }

    public void AddScore(int val, int side)
    {
        if (side == 0)
        {
            team1Score += val;
            team1ScoreText.text = "Team 1 Score : " + team1Score;
        }
        if (side == 1)
        {
            team2Score += val;
            team2ScoreText.text = "Team 2 Score : " + team2Score;
        }

        if (team1Score == maxScore || team2Score == maxScore)
        {
            gameObject.GetComponent<GameOver> ().OpenGameOverCanvas("Soccer Game");
        }
    }
}
