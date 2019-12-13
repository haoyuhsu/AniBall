using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        /* 玩家碰到Grenade時, 去改變玩家的狀態(isBomb = True) */
        if (col.tag == "Player")
        {
            Transform player = col.transform.parent;
            SpecialEffect se = player.GetComponent<SpecialEffect> ();
            se.GrenadeTrigger();
            Destroy(gameObject);
        }
    }
}
