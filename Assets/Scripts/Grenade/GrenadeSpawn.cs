using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeSpawn : MonoBehaviour
{
    public GameObject grenade;
    float xPos;
    float zPos;
    Quaternion grenadeRotation = Quaternion.Euler (0, 0, -22f);
    float waitTime;
    public float min = 5.0f;
    public float max = 10.0f;
    void Start()
    {
        StartCoroutine("GenerateGrenade");
    }

    IEnumerator GenerateGrenade()
    {
        while(true) {
            xPos = Random.Range(5.0f, -5.0f);
            zPos = Random.Range(5.0f, -5.0f);
            Instantiate(grenade, new Vector3(xPos, 1f, zPos), grenadeRotation);
            waitTime = Random.Range(min, max);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
