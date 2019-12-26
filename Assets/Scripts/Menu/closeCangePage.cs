using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeCangePage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Animation>().Play("changePage");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void closecp(){
        this.gameObject.SetActive(false);
    }
}
