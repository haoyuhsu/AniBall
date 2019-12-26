using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canvasAttach : MonoBehaviour
{
    public Canvas menu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 CanvasPos = Camera.main.WorldToScreenPoint(this.transform.position);
        menu.transform.position = CanvasPos;
    }
}
