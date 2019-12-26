using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenAbout : MonoBehaviour
{
    // Start is called before the first frame updat
    public Animation m_ani;
    public GameObject AboutPanel;
    public GameObject fogPanel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void openAni()
    {
        AboutPanel.SetActive(true);
        fogPanel.SetActive(true);
        transform.position = new Vector3(Screen.width/2f, Screen.height/2f, 1);
        m_ani.Play("OpenAboutSc");
    }
}
