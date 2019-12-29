using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int team1Score = 0;
    public int team2Score = 0;
    public Image[] Team1ScoreImage;
    public Image[] Team2ScoreImage;
    public Sprite OrangeSprite;
    public Sprite nonOrangeSprite;
    int maxScore;

    void Start()
    {
        maxScore = FindObjectOfType<GameSetting> ().maxScore;
        SpriteInit();
    }

    void SpriteInit()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i < maxScore)
            {
                Team1ScoreImage[i].enabled = true;
                Team1ScoreImage[i].sprite = nonOrangeSprite;
                Team2ScoreImage[i].enabled = true;
                Team2ScoreImage[i].sprite = nonOrangeSprite;
            }
            else
            {
                Team1ScoreImage[i].enabled = false;
                Team2ScoreImage[i].enabled = false;
            }
        }
    }

    public void AddScore(int val, int side)
    {
        if (side == 0)
        {
            Team1ScoreImage[team1Score].sprite = OrangeSprite;
            team1Score += val;
        }
        if (side == 1)
        {
            Team2ScoreImage[team2Score].sprite = OrangeSprite;
            team2Score += val;
        }

        if (team1Score == maxScore || team2Score == maxScore)
        {
            gameObject.GetComponent<GameOver> ().OpenGameOverCanvas("Soccer Game");
        }
    }

    /*void SetSprite(int side)
    {
        if (side == 0)
        {
            Team1ScoreImage[team1Score].sprite = OrangeSprite;
        }
        if (side == 1)
        {
            Team2ScoreImage[team2Score].sprite = OrangeSprite;
        }
    }*/
}
