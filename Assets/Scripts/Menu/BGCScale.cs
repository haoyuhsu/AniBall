using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGCScale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(Screen.width/1200f, Screen.height/425f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
