using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playOnes : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioc; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playo(){
        audioSource.PlayOneShot(audioc);
    }
}
