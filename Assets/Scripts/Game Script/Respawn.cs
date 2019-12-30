using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public MusicController musicController;
    public bool isSoccerGame = false;
    BallRespawn ballRespawn;
    Barrel barrel;

    /* 任何碰到Respawn Area的物品都會觸發 */
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            ballRespawn = col.gameObject.GetComponent<BallRespawn>();
            if (!ballRespawn.isRespawning)
            {
                ballRespawn.isRespawning = true;
                ballRespawn.RespawnPlayer();   
                if (isSoccerGame)
                    musicController.PlayWaterDrop();
            }
        }
        if (col.gameObject.tag == "Barrel")
        {
            barrel = col.gameObject.GetComponent<Barrel>();
            if (!barrel.isRespawning)
            {
                barrel.isRespawning = true;
                barrel.resetBarrel("center", 2.0f, false);
                if (isSoccerGame)
                    musicController.PlayWaterDrop();
            }
        }
        if (col.gameObject.tag == "Floor")
        {
            Destroy(col.gameObject);
        }
    }
}
