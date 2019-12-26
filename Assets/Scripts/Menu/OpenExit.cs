using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenExit : MonoBehaviour
{
    public GameObject ExitPanel;
    public GameObject fogPanel;
    public Animation m_ani;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void openep(){
        ExitPanel.SetActive(true);
        fogPanel.SetActive(true);
        m_ani.Play("OpenAboutSc");
    }
    public void closeep(){
        ExitPanel.SetActive(false);
        fogPanel.SetActive(false);
    }
    public void exitGame(){
        Application.Quit();
    }
}
