using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    DeathCount[] playersLeft;

    public void CheckWinner()
    {
        playersLeft = FindObjectsOfType<DeathCount> ();
        if (playersLeft.Length == 1)
        {
            gameObject.GetComponent<GameOver> ().OpenGameOverCanvas("Survival Game");
        }
    }
}
