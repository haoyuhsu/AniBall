using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetting : MonoBehaviour
{
    public int numPlayers = 2;
    public int roundTime = 30;
    public int maxScore = 3;
    public int maxDeath = 3;
    public string[] playersName = new string[4];                    // 玩家的名稱
    public string[] playersAnimal = new string[4];                  // 玩家要選的動物
    public KeyCode[] KeyCodeList = new KeyCode[4];                  // 玩家的噴射按鍵
    public Color[] ColorList = new Color[4];                        // 玩家各自代表色 (紅 藍 綠 黃)
    public ParticleSystem[] ParticleList = new ParticleSystem[4];   // 玩家各自圓環 (紅 藍 綠 黃)
    static GameSetting TheOnlyOneGameSetting;

    void Awake()
    {
        if (TheOnlyOneGameSetting != null)
        {
            Destroy(this.gameObject);
            return;
        }

        TheOnlyOneGameSetting = this;
        GameObject.DontDestroyOnLoad( this.gameObject );

        InitPlayersName();
    }

    void InitPlayersName()
    {
        playersName[0] = "Player 1";
        playersName[1] = "Player 2";
        playersName[2] = "Player 3";
        playersName[3] = "Player 4";
    }
}