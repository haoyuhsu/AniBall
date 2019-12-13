using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        /*if (col.tag == "Player")
        {
            BallScore ballScore = col.GetComponent<BallScore> ();
            ballScore.AddScore(1);
        }
        Destroy(gameObject);*/
    }
}
