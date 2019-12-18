using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRespawn : MonoBehaviour
{
    FloorController floorController;
    public string TypeOfGame;
    float respawnHeight = 5.0f;
    float xPos;
    float zPos;
    Transform tf;
    Rigidbody rb;
    Vector3 spawnPos;             // 重生位置
    float groundHeight;           // 地板高度
    DeathCount deathCount;
    bool isRespawning = false;

    void Start()
    {
        tf = gameObject.GetComponent<Transform> ();
        rb = gameObject.GetComponent<Rigidbody> ();
        deathCount = gameObject.GetComponent<DeathCount> ();
        floorController = FindObjectOfType<FloorController> ();
        groundHeight = floorController.transform.position.y;
    }

    void FixedUpdate()
    {
        // 高度低於某個值時, 就重生
        if (tf.position.y <= groundHeight-10 && !isRespawning)
        {
            //Debug.Log(tf.position.y);
            isRespawning = true;
            RespawnPlayer();
        }
    }

    public void RespawnPlayer()
    {
        if (TypeOfGame == "Survival Game")     // 生存模式重生
        {
            rb.velocity = new Vector3(0, 0, 0);   // 速度歸零, 避免慣性移動
            spawnPos = floorController.GetRandomFloorPosition();  // 從 FloorController 去抓取目前存在地板的位置, 避免重生在空的地板上
            spawnPos += new Vector3(0, respawnHeight, 0);
            tf.position = spawnPos;
            if (deathCount != null)
                deathCount.AddDeath();
        }
        if (TypeOfGame == "Soccer Game")       // 取分模式重生
        {
            rb.velocity = new Vector3(0, 0, 0);   // 速度歸零, 避免慣性移動
            spawnPos = floorController.GetRandomFloorPosition();  // 從 FloorController 去抓取目前存在地板的位置, 避免重生在空的地板上
            spawnPos += new Vector3(0, respawnHeight, 0);
            tf.position = spawnPos;
        }
        isRespawning = false;
    }

    public void SetGroundHeight(float val)
    {
        groundHeight = val;
    }

}
