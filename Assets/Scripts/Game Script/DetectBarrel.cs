using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBarrel : MonoBehaviour
{
    public int side;     // 0: Team1, 1: Team2
    public Score soccerScore;
    public MusicController musicController;
    public ParticleSystem Fireworks;

    void OnTriggerEnter(Collider col)
    {
        /* 如果Barrel碰觸Trigger到的話, 就加分並Reset Barrel位置 */
        if (col.tag == "Barrel")
        {
            Barrel barrel = col.GetComponent<Barrel> ();
            if (!barrel.isRespawning)
            {
                barrel.isRespawning = true;
                soccerScore.AddScore(1, side);
                musicController.PlayGoalClip();
                musicController.PlayRefWhistle();
                Fireworks.Play();
                barrel.resetBarrel("center", 5f, true);
            }
        }
    }

}
