using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        /* 玩家碰到Magnet時, 去改變玩家的狀態(isBall = false), 這樣就可以去用磁極影響其他玩家 */
        if (col.tag == "Player")
        {
            Transform player = col.transform.parent;
            SpecialEffect se = player.GetComponent<SpecialEffect> ();
            se.MagnetTrigger();
            Destroy(gameObject);
        }
    }
}
