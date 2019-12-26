using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chooseIconPos : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject backBtn;
    public AudioSource audioSource;
    public AudioClip audioc; 
    void Start()
    {
        transform.position = new Vector3(Screen.width/2f - 45*Screen.width/800f, Screen.height/1.8f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPlay(){
        transform.position = new Vector3(Screen.width/2f - 45*Screen.width/800f, Screen.height/1.8f, 1);
        
        audioSource.PlayOneShot(audioc, 0.5F);
    }
    public void OnManual(){
        transform.position = new Vector3(Screen.width/2f - 45*Screen.width/800f, Screen.height/1.8f - 2*Screen.width/15f, 1);
        audioSource.PlayOneShot(audioc, 0.5F);
    }
    public void OnAbout(){
        transform.position = new Vector3(Screen.width/2f - 45*Screen.width/800f, Screen.height/1.8f - Screen.width/15f, 1);
        audioSource.PlayOneShot(audioc, 0.5F);
    }
    public void OnExit(){
        transform.position = new Vector3(Screen.width/2f - 45*Screen.width/800f, Screen.height/1.8f - 3*Screen.width/15f, 1);
        audioSource.PlayOneShot(audioc, 0.5F);
    }
}
