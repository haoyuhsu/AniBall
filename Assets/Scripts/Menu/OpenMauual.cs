using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMauual : MonoBehaviour
{
    public GameObject ManualPanel;
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
    public void openmanual()
    {
        ManualPanel.SetActive(true);
        fogPanel.SetActive(true);
        m_ani.Play("OpenAboutSc");
    }
    public void closemanual()
    {
        ManualPanel.SetActive(false);
        fogPanel.SetActive(false);
    }
}
