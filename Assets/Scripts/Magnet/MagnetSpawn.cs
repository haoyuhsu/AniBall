using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetSpawn : MonoBehaviour
{
    public GameObject magnet;
    FloorController floorController;
    Vector3 spawnPos;
    float respawnHeight = 1.0f;
    Quaternion magnetRotation = Quaternion.Euler (0, 0, -22f);
    float waitTime;
    public float min = 10.0f;
    public float max = 20.0f;
    void Start()
    {
        floorController = GetComponent<FloorController> ();
        StartCoroutine("GenerateMagnet");
    }

    IEnumerator GenerateMagnet()
    {
        while(true) {
            spawnPos = floorController.GetRandomFloorPosition();  // 從 FloorController 去抓取目前存在地板的位置, 避免重生在空的地板上
            spawnPos += new Vector3(0, respawnHeight, 0);
            Instantiate(magnet, spawnPos, magnetRotation);
            waitTime = Random.Range(min, max);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
