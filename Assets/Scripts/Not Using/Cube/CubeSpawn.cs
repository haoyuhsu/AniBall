using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawn : MonoBehaviour
{
    public GameObject cube;
    float xPos;
    float zPos;
    Quaternion cubeRotation = Quaternion.Euler (-12f, -31f, 30f);
    float waitTime;
    public float min = 1.0f;
    public float max = 3.0f;
    void Start()
    {
        StartCoroutine("GenerateCube");
    }

    IEnumerator GenerateCube()
    {
        while(true) {
            xPos = Random.Range(5.0f, -5.0f);
            zPos = Random.Range(5.0f, -5.0f);
            Instantiate(cube, new Vector3(xPos, 1f, zPos), cubeRotation);
            waitTime = Random.Range(min, max);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
