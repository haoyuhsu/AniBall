using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathCount : MonoBehaviour
{
    public int lifeLeft = 0;
    public int playerIndex;
    Death death;
    LifeUI lifeUI;
    int maxDeath;

    void Start()
    {
        death = FindObjectOfType<Death> ();
        lifeUI = FindObjectOfType<LifeUI> ();
        maxDeath = FindObjectOfType<GameSetting> ().maxDeath;
        lifeLeft = maxDeath;
    }

    public void AddDeath()
    {
        lifeLeft -= 1;
        lifeUI.CloseLifeImage(playerIndex, lifeLeft);
        if (lifeLeft == 0)
        {
            gameObject.SetActive(false);
            death.CheckWinner();
        }
    }
}
