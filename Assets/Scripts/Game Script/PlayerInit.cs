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
    public GameObject soccerCat;
    public GameObject survivalCat;
    public GameObject soccerDog;
    public GameObject survivalDog;
    
    public string TypeOfGame;
    public GameObject PlayersObject;    // 玩家們
    int numPlayers;
    GameSetting gameSetting;
    FloorController floorController;

    public GameObject Magnets;
    public GameObject spotlight;
    public GameObject spotlightObject;

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
        Debug.Log(numPlayers);
        for (int i=0; i<numPlayers; i++)
        {
            string animalName = gameSetting.playersAnimal[i];
            if (animalName == "Fox")  GeneratePlayer_Soccer(i, soccerFox);
            else if (animalName == "Racoon")  GeneratePlayer_Soccer(i, soccerRacoon);
            else if (animalName == "Cat")  GeneratePlayer_Soccer(i, soccerCat);
            else if (animalName == "Dog")  GeneratePlayer_Soccer(i, soccerDog);
        }
    }

    void SurvivalGameInit()
    {
        for (int i=0; i<numPlayers; i++)
        {
            string animalName = gameSetting.playersAnimal[i];
            if (animalName == "Fox")  GeneratePlayer(i, survivalFox);
            else if (animalName == "Racoon")  GeneratePlayer(i, survivalRacoon);
            else if (animalName == "Cat")  GeneratePlayer(i, survivalCat);
            else if (animalName == "Dog")  GeneratePlayer(i, survivalDog);
        }
    }

    void GeneratePlayer(int index, GameObject animalToSpawn)
    {
        Vector3 spawnPos = floorController.GetRandomFloorPosition();                           // 取得隨機生成的位置
        spawnPos += new Vector3(0, 1.5f, 0);                                                   // 將位置以y軸做offset, 拉高生成高度
        GameObject animalPrefab = Instantiate(animalToSpawn, spawnPos, Quaternion.identity);   // 生成Animal Prefab物件
        animalPrefab.transform.name = gameSetting.playersName[index];                          // 改Prefab名稱為原本設定的玩家名稱

        /* 更改玩家代表顏色及圓環 */
        animalPrefab.GetComponent<PlayerColor> ().myColor = gameSetting.ColorList[index];
        animalPrefab.GetComponent<Ring> ().ringParticle = gameSetting.ParticleList[index];
        
        /* 更改玩家的控制按鍵 */
        BallMove ballMove = animalPrefab.GetComponent<BallMove> ();
        ballMove.vertical_axis = "Player" + (index+1) + "_Vertical";
        ballMove.horizontal_axis = "Player" + (index+1) + "_Horizontal";
        ballMove.shoot = gameSetting.KeyCodeList[index];

        /* 將此Prefab放到Players的Hierarchy下面 */
        animalPrefab.transform.parent = PlayersObject.transform;
    }

    void GeneratePlayer_Soccer(int index, GameObject animalToSpawn){
        Vector3 spawnPos = Vector3.zero;
        
        if(numPlayers==2){
            if(index==0){
                spawnPos = Magnets.transform.GetChild(0).position;
                spawnPos += Magnets.transform.GetChild(1).position;
                spawnPos /= 2;
            }
            if(index==1){
                spawnPos = Magnets.transform.GetChild(2).position;
                spawnPos += Magnets.transform.GetChild(3).position;
                spawnPos /= 2;
            }
        }
        else if(numPlayers==4){
            if(index==1){
                spawnPos = Magnets.transform.GetChild(2).position;
            }
            else if(index==2){
                spawnPos = Magnets.transform.GetChild(1).position;
            }
            else {
                spawnPos = Magnets.transform.GetChild(index).position;
            }
        }
        spawnPos += new Vector3(0, 1.5f, 0);                                                   // 將位置以y軸做offset, 拉高生成高度
        GameObject animalPrefab = Instantiate(animalToSpawn, spawnPos, Quaternion.identity);   // 生成Animal Prefab物件
        GameObject SpotlightPrefab = Instantiate(spotlight,spotlightObject.transform);
        Spotlight spotlightComponent = SpotlightPrefab.GetComponent<Spotlight>();
        spotlightComponent.Animal = animalPrefab;
        SpotlightPrefab.SetActive(false);
        animalPrefab.transform.name = gameSetting.playersName[index];                          // 改Prefab名稱為原本設定的玩家名稱

        /* 更改玩家代表顏色及圓環 */
        animalPrefab.GetComponent<PlayerColor> ().myColor = gameSetting.ColorList[index];
        animalPrefab.GetComponent<Ring> ().ringParticle = gameSetting.ParticleList[index];
        
        /* 更改玩家的控制按鍵 */
        BallMove ballMove = animalPrefab.GetComponent<BallMove> ();
        ballMove.vertical_axis = "Player" + (index+1) + "_Vertical";
        ballMove.horizontal_axis = "Player" + (index+1) + "_Horizontal";
        ballMove.shoot = gameSetting.KeyCodeList[index];

        /* 將此Prefab放到Players的Hierarchy下面 */
        animalPrefab.transform.parent = PlayersObject.transform;
    }

}
