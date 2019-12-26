using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(Screen.height/500f, Screen.height/500f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
