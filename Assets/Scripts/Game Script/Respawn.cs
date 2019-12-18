using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    /* 任何碰到Respawn Area的物品都會觸發 */
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<BallRespawn>().RespawnPlayer();   
        }
        if (col.gameObject.tag == "Barrel")
        {
           col.gameObject.GetComponent<Barrel>().resetBarrel("center");
        }
        if (col.gameObject.tag == "Floor")
        {
            Destroy(col.gameObject);
        }
    }
}
