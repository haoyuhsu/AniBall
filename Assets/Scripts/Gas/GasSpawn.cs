using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasSpawn : MonoBehaviour
{
    public GameObject gas;
    FloorController floorController;
    Vector3 spawnPos;
    float respawnHeight = 1.0f;
    Quaternion gasRotation = Quaternion.Euler (0, 0, -22f);
    float waitTime;
    public float min = 5.0f;
    public float max = 10.0f;
    void Start()
    {
        floorController = GetComponent<FloorController> ();
        StartCoroutine("GenerateGas");
    }

    IEnumerator GenerateGas()
    {
        while(true) {
            spawnPos = floorController.GetRandomFloorPosition();  // 從 FloorController 去抓取目前存在地板的位置, 避免重生在空的地板上
            spawnPos += new Vector3(0, respawnHeight, 0);
            Instantiate(gas, spawnPos, gasRotation);
            waitTime = Random.Range(min, max);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
