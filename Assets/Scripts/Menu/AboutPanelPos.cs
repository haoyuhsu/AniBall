using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutPanelPos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Screen.width/2f, Screen.height/2f, 1); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void pasc(){
        transform.localScale = new Vector3(Screen.height/420f, Screen.height/420f, 1);
    }
    void playnext(){
        gameObject.GetComponent<Animation>().Play("OpenAboutSk");
    }
}
