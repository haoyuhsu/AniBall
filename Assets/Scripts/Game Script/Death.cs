using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    DeathCount[] playersLeft;

    public void CheckWinner()
    {
        playersLeft = FindObjectsOfType<DeathCount> ();
        GameOver gameOverobj = gameObject.GetComponent<GameOver>();
        if (playersLeft.Length == 1 && !gameOverobj.Ended)
        {
            gameOverobj.OpenGameOverCanvas("Survival Game");
        }
    }
}
