using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        /* 玩家碰到Gas時, 去改變玩家的狀態(isGas = True) */
        if (col.tag == "Player")
        {
            Transform player = col.transform.parent;
            SpecialEffect se = player.GetComponent<SpecialEffect> ();
            se.GasTrigger();
            Destroy(gameObject);
        }
    }
}
