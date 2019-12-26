using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spotlight : MonoBehaviour
{
    public GameObject Animal;

    // Update is called once per frame
    void Update()
    {
        transform.position = Animal.transform.position + new Vector3(0,2.5f,0);
        transform.LookAt(Animal.transform.position);
    }
}
