using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeSpawn : MonoBehaviour
{
    public GameObject grenade;
    FloorController floorController;
    Vector3 spawnPos;
    float respawnHeight = 1.0f;
    Quaternion grenadeRotation = Quaternion.Euler (0, 0, -22f);
    float waitTime;
    public float min = 5.0f;
    public float max = 10.0f;
    void Start()
    {
        floorController = GetComponent<FloorController> ();
        StartCoroutine("GenerateGrenade");
    }

    IEnumerator GenerateGrenade()
    {
        while(true) {
            spawnPos = floorController.GetRandomFloorPosition();  // 從 FloorController 去抓取目前存在地板的位置, 避免重生在空的地板上
            spawnPos += new Vector3(0, respawnHeight, 0);
            Instantiate(grenade, spawnPos, grenadeRotation);
            waitTime = Random.Range(min, max);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
