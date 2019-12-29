using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public MusicController musicController;
    FloorController floorController;
    Vector3 originalPos;
    Quaternion originalRot;
    Transform barrelTransform;
    Rigidbody rb;
    Vector3 player1Side = new Vector3(-1.9f, 2.93f, -1.85f);    // 重生位置靠近Player 1
    Vector3 player2Side = new Vector3(2.64f, 2.93f, -1.85f);    // 重生位置靠近Player 2
    Vector3 spawnPos;             // 重生位置
    float groundHeight;           // 地板高度
    public bool isRespawning = false;
    public GameObject Players;
 
    void Start()
    {
        rb = GetComponent<Rigidbody> ();
        barrelTransform = GetComponent<Transform> ();
        originalPos = barrelTransform.position;
        originalRot = barrelTransform.rotation;
        floorController = FindObjectOfType<FloorController> ();
        groundHeight = floorController.transform.position.y;

        musicController.PlayGameStartWhistle();
    }

    void FixedUpdate()
    {
        /* Barrel 掉落到場外, Reset其位置為中間 */
        if (barrelTransform.position.y <= groundHeight-10 && !isRespawning)
        {
            isRespawning = true;
            resetBarrel("center", 2.0f, false);
        }
    }

    public void resetBarrel(string playerSide, float waitTime, bool respawnPlayer)
    {
        /* 等待2秒後再Reset Barrel位置 */
        StartCoroutine(ToResetBarrel(playerSide, waitTime, respawnPlayer));
    }

    IEnumerator ToResetBarrel(string playerSide, float waitTime, bool respawnPlayer)
    {
        isRespawning = true;
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
        if (respawnPlayer)
        {
            RespawnPlayer();
        }
        musicController.PlayGameStartWhistle();
        isRespawning = false;
    }

    void RespawnPlayer()
    {
        foreach (Transform child in Players.transform)
        {
            BallRespawn ballRespawn = child.gameObject.GetComponent<BallRespawn> ();
            if (ballRespawn != null)
                ballRespawn.RespawnPlayer();
        }
    }

    void OnCollisionEnter(Collision col)             // 物體掉落至場景外的情況
    {
        if (col.gameObject.tag == "environment")
        {
            isRespawning = true;
            resetBarrel("center", 2.0f, false);
        }
    }

}
