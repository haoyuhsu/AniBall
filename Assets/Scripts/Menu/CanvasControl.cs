using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasControl : MonoBehaviour
{
    public GameObject[] Canvases ;
    private int curcanvas = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    } 
    public void changeCV(int i){
        Canvases[curcanvas].SetActive(false);
        Canvases[i].SetActive(true);
        curcanvas = i;
    }
}
