using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathCount : MonoBehaviour
{
    public int deathCount = 0;
    string playerName;
    Death death;
    int maxDeath;

    void Start()
    {
        playerName = gameObject.name;
        death = FindObjectOfType<Death> ();
        maxDeath = FindObjectOfType<GameSetting> ().maxDeath;
    }

    public void AddDeath()
    {
        deathCount += 1;
        if (deathCount == maxDeath)
        {
            gameObject.SetActive(false);
            death.CheckWinner();
        }
    }
}
