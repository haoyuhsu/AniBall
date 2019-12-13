using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasSpawn : MonoBehaviour
{
    public GameObject gas;
    float xPos;
    float zPos;
    Quaternion gasRotation = Quaternion.Euler (0, 0, -22f);
    float waitTime;
    public float min = 5.0f;
    public float max = 10.0f;
    void Start()
    {
        StartCoroutine("GenerateGas");
    }

    IEnumerator GenerateGas()
    {
        while(true) {
            xPos = Random.Range(5.0f, -5.0f);
            zPos = Random.Range(5.0f, -5.0f);
            Instantiate(gas, new Vector3(xPos, 1f, zPos), gasRotation);
            waitTime = Random.Range(min, max);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
