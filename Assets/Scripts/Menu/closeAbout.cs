using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeAbout : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Abpn;
    public GameObject fogpn;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void closeP(){
        Abpn.SetActive(false);
        fogpn.SetActive(false);
    }
}
