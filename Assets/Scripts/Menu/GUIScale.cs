using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIScale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(Screen.width/1600f + Screen.height/1600f, Screen.width/1600f + Screen.height/1600f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
