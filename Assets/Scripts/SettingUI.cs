
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;


public class SettingUI : MonoBehaviour
{
    public Text numPlayersTxt;
    public Text maxScoreTxt;
    public Text maxDeathTxt;
    public Text TimeTxt;
    public Sprite[] AnimalsSprite;
    public Sprite[] AnimalsNameSprite;
    public Image[] AnimalsImg;
    public int[] playersAnimalsIndex;
    public Image[] AnimalsNameImg;
    public Button[] chooseBtn;
    public InputField[] PlayersNameList = new InputField[4];
    GameSetting gameSetting;

    void Start()
    {
        gameSetting = FindObjectOfType<GameSetting> ();
        InitParameter();
    }
    void InitParameter()
    {
        SetNumPlayer();
        SetMaxScore();
        SetMaxDeath();
        SetTimeLimit();
        for (int i=0; i<4; i++)
        {
            SetAnimal(i);
        }
    }

    public void SetNumPlayer()
    {
        if (numPlayersTxt != null)
        {
            string num = numPlayersTxt.text;
            gameSetting.numPlayers = int.Parse(num);
        }
    }

    public void SetMaxScore()
    {
        if (maxScoreTxt != null)
        {
            string num = maxScoreTxt.text;
            gameSetting.maxScore = int.Parse(num);
        }
    }

    public void SetMaxDeath()
    {
        if (maxDeathTxt != null)
        {
            string num = maxDeathTxt.text;
            gameSetting.maxDeath = int.Parse(num);
        }
    }

    public void SetTimeLimit()
    {
        if (TimeTxt)
        {
            string num = TimeTxt.text;
            gameSetting.roundTime = int.Parse(num);
        }
    }

    public void SetAnimal(int index)
    {
        if(playersAnimalsIndex[index] == 0)gameSetting.playersAnimal[index] = "fox";
        else if(playersAnimalsIndex[index] == 1)gameSetting.playersAnimal[index] = "cat";
        else if(playersAnimalsIndex[index] == 2)gameSetting.playersAnimal[index] = "dog";
        else if(playersAnimalsIndex[index] == 3)gameSetting.playersAnimal[index] = "raccoon";
        SetPlayerAnimalImg(index);
    }

    public void SetPlayersName(int index)
    {
        gameSetting.playersName[index] = PlayersNameList[index].text;
    }
    public void SetGameMode(int mode){
        //gameSetting.gameMode = mode;
    }
    public void SetPlayerBtn(int index)
    {
        if(gameSetting.numPlayers + index>=2 && gameSetting.numPlayers + index<=4){
            gameSetting.numPlayers += index;
            numPlayersTxt.text = gameSetting.numPlayers.ToString();
        }
    }
    public void SetLifeBtn(int index)
    {
        if(gameSetting.maxDeath + index>0){
            gameSetting.maxDeath += index;
            maxDeathTxt.text = gameSetting.maxDeath.ToString();
        }
    }
    public void SetTimeBtn(int index)
    {
        if(gameSetting.roundTime + index>0){
            gameSetting.roundTime += index;
            TimeTxt.text = gameSetting.roundTime.ToString();
        }
    }
    public void SetAnimal1Btn(int index){
        if(playersAnimalsIndex[0] + index >=0 && playersAnimalsIndex[0] + index<=3)
        playersAnimalsIndex[0] += index;
        SetAnimal(0);
        SetPlayerAnimalImg(0);
    }
    public void SetAnimal2Btn(int index){
        if(playersAnimalsIndex[1] + index >=0 && playersAnimalsIndex[1] + index<=3)
        playersAnimalsIndex[1] += index;
        SetAnimal(1);
        SetPlayerAnimalImg(1);
    }
    public void SetAnimal3Btn(int index){
        if(playersAnimalsIndex[2] + index >=0 && playersAnimalsIndex[2] + index<=3)
        playersAnimalsIndex[2] += index;
        SetAnimal(2);
        SetPlayerAnimalImg(2);
    }
    public void SetAnimal4Btn(int index){
        if(playersAnimalsIndex[3] + index >=0 && playersAnimalsIndex[3] + index<=3)
        playersAnimalsIndex[3] += index;
        SetAnimal(3);
        SetPlayerAnimalImg(3);
    }
    public void SetPlayerAnimalImg(int index){
        AnimalsImg[index].sprite = AnimalsSprite[playersAnimalsIndex[index]];
        AnimalsNameImg[index].sprite = AnimalsNameSprite[playersAnimalsIndex[index]];
    }
}