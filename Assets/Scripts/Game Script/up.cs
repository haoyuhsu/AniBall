using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class up : MonoBehaviour
{
    float cnt = 0;
    float speed = 0.1f;
    public GameObject obj;
    bool able = true;
    void Start()
    {
        Time.timeScale = 0;
        //obj.GetComponent<FallFloor>().enable = false;
    }

    void Update()
    {
        if(cnt<=5){
            cnt += speed;
        }
        else if(cnt<=50){
            transform.Translate((new Vector3(0,speed,0)));
            cnt += speed;
        }
        else if(able){
            obj.GetComponent<FallFloor>().enable();
            Time.timeScale = 1;
            able = false;
        }
    }
}
