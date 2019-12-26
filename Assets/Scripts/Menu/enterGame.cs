using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class enterGame : MonoBehaviour
{
    public GameSetting gg;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void enterG(){
        if(gg.gameMode == 0){
            SceneManager.LoadScene("SoccerGame");
        }
        else{
            SceneManager.LoadScene("SurvivalGame");
        }
    }
}
