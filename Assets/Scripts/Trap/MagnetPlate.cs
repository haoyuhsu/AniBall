using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPlate : MonoBehaviour
{
    /* 碰到玩家時, 會改變玩家的磁極 */
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            Magnetic magnetic = col.gameObject.GetComponent<Magnetic> ();
            magnetic.ToggleMagnetPole();
        }
    }
}
