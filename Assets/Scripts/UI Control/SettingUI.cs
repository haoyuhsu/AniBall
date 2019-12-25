using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public Dropdown numPlayersDropDown;
    public Dropdown maxScoreDropDown;
    public Dropdown maxDeathDropDown;
    public Dropdown TimeDropDown;
    public InputField[] PlayersNameList = new InputField[4];
    public Dropdown[] AnimalsDropDown = new Dropdown[4];
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
        for (int i=0; i<AnimalsDropDown.Length; i++)
        {
            SetAnimal(i);
        }
    }

    public void SetNumPlayer()
    {
        if (numPlayersDropDown != null)
        {
            string num = numPlayersDropDown.options[numPlayersDropDown.value].text;
            gameSetting.numPlayers = int.Parse(num);
        }
    }

    public void SetMaxScore()
    {
        if (maxScoreDropDown != null)
        {
            string num = maxScoreDropDown.options[maxScoreDropDown.value].text;
            gameSetting.maxScore = int.Parse(num);
        }
    }

    public void SetMaxDeath()
    {
        if (maxDeathDropDown != null)
        {
            string num = maxDeathDropDown.options[maxDeathDropDown.value].text;
            gameSetting.maxDeath = int.Parse(num);
        }
    }

    public void SetTimeLimit()
    {
        if (TimeDropDown)
        {
            string num = TimeDropDown.options[TimeDropDown.value].text;
            gameSetting.roundTime = int.Parse(num);
        }
    }

    public void SetAnimal(int index)
    {
        Dropdown animalDD = AnimalsDropDown[index];
        if (animalDD != null)
        {
            gameSetting.playersAnimal[index] = animalDD.options[animalDD.value].text;
        }
    }

    public void SetPlayersName(int index)
    {
        gameSetting.playersName[index] = PlayersNameList[index].text;
    }
}
