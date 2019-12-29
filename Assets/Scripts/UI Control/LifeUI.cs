using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    public LifeObject[] LifeObjects;
    GameSetting gameSetting;

    void Start()
    {
        gameSetting = FindObjectOfType<GameSetting> ();
        SetLifeUI();
    }

    void SetLifeUI()
    {
        int numPlayers = gameSetting.numPlayers;
        int maxDeath = gameSetting.maxDeath;
        for (int i = 0; i < numPlayers; i++)
        {
            for (int j = 0; j < maxDeath; j++)
            {
                LifeObjects[i].LifeImages[j].gameObject.SetActive(true);
            }
        }
    }

    public void CloseLifeImage(int index, int lifeLeft)
    {
        LifeObjects[index].LifeImages[lifeLeft].enabled = false;
    }
}
