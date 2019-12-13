using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    Vector3 originalPos;
    Quaternion originalRot;
    Transform barrelTransform;
    Rigidbody rb;
    Vector3 player1Side = new Vector3(-1.9f, 2.93f, -1.85f);    // 重生位置靠近Player 1
    Vector3 player2Side = new Vector3(2.64f, 2.93f, -1.85f);    // 重生位置靠近Player 2
 
    void Start()
    {
        rb = GetComponent<Rigidbody> ();
        barrelTransform = GetComponent<Transform> ();
        originalPos = barrelTransform.position;
        originalRot = barrelTransform.rotation;
    }

    void FixedUpdate()
    {
        /* Barrel 掉落到場外, Reset其位置為中間 */
        if (barrelTransform.position.y <= -10f)
            resetBarrel("center");
    }

    public void resetBarrel(string playerSide)
    {
        /* 等待2秒後再Reset Barrel位置 */
        StartCoroutine(ToResetBarrel(playerSide, 2.0f));
    }

    IEnumerator ToResetBarrel(string playerSide, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        rb.velocity = new Vector3(0, 0, 0);            // 速度歸零, 避免慣性
        barrelTransform.rotation = originalRot;
        /* 根據playerside參數去調整, */
        if (playerSide == "player1")
        {
            barrelTransform.position = player1Side;
        }
        if (playerSide == "player2")
        {
            barrelTransform.position = player2Side;
        }
        if (playerSide == "center")
        {
            barrelTransform.position = originalPos;
        }
    }

}
