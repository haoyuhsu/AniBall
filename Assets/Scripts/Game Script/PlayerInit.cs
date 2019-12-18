using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInit : MonoBehaviour
{
    /* 做玩家Prefab的動態生成 */
    public GameObject soccerFox;
    public GameObject survivalFox;
    public GameObject soccerRacoon;
    public GameObject survivalRacoon;
    public string TypeOfGame;
    public GameObject PlayersObject;    // 玩家們
    int numPlayers;
    GameSetting gameSetting;
    FloorController floorController;

    void Start()
    {
        gameSetting = FindObjectOfType<GameSetting> ();
        floorController = GetComponent<FloorController> ();
        numPlayers = gameSetting.numPlayers;
        if (TypeOfGame == "Survival Game")
        {
            SurvivalGameInit();
        }
        if (TypeOfGame == "Soccer Game")
        {
            SoccerGameInit();
        }
    }

    void SoccerGameInit()
    {
        for (int i=0; i<numPlayers; i++)
        {
            string animalName = gameSetting.playersAnimal[i];
            if (animalName == "Fox")  GeneratePlayer(i, soccerFox);
            else if (animalName == "Racoon")  GeneratePlayer(i, soccerRacoon);
        }
    }

    void SurvivalGameInit()
    {
        for (int i=0; i<numPlayers; i++)
        {
            string animalName = gameSetting.playersAnimal[i];
            if (animalName == "Fox")  GeneratePlayer(i, survivalFox);
            else if (animalName == "Racoon")  GeneratePlayer(i, survivalRacoon);
        }
    }

    void GeneratePlayer(int index, GameObject animalToSpawn)
    {
        Vector3 spawnPos = floorController.GetRandomFloorPosition();                           // 取得隨機生成的位置
        spawnPos += new Vector3(0, 1.5f, 0);                                                   // 將位置以y軸做offset, 拉高生成高度
        GameObject animalPrefab = Instantiate(animalToSpawn, spawnPos, Quaternion.identity);   // 生成Animal Prefab物件
        animalPrefab.transform.name = gameSetting.playersName[index];                          // 改Prefab名稱為原本設定的玩家名稱
        
        /* 更改玩家的控制按鍵 */
        BallMove ballMove = animalPrefab.GetComponent<BallMove> ();
        ballMove.vertical_axis = "Player" + (index+1) + "_Vertical";
        ballMove.horizontal_axis = "Player" + (index+1) + "_Horizontal";
        ballMove.shoot = gameSetting.KeyCodeList[index];

        /* 將此Prefab放到Players的Hierarchy下面 */
        animalPrefab.transform.parent = PlayersObject.transform;
    }

}
