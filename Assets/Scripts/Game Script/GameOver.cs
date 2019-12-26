using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Canvas gameoverCanvas;
    public Text resultText;
    public Score soccerScore;
    DeathCount deathCount;

    GameObject Winner = null;
    public GameObject Camera;
    public float RotateSpeed;
    public float CameraDis;
    public GameObject Spotlights;
    public GameObject Directlight;
    float Angle = 0;
    Vector3 WinnerPos;
    float Distance;
    float Radius;

    void Start()
    {
        gameoverCanvas.enabled = false;     // 初始時把結束Canvas關閉
    }

    void FixedUpdate(){
        if(Winner!=null){
            WinnerPos = Winner.transform.position;
            Distance = Vector3.Distance(WinnerPos , Camera.transform.position);
            Radius = Vector2.Distance(new Vector2(WinnerPos.x,WinnerPos.z),new Vector2(Camera.transform.position.x,Camera.transform.position.z));
            //Debug.Log("dis "+Distance);
        }
    }

    public void OpenGameOverCanvas(string TypeOfGame)
    {
        //Time.timeScale = 0f;    // 結束時將時間停止

        /* 根據遊戲類型去決定Game Over形式 */
        if (TypeOfGame == "Soccer Game")
        {
            if (soccerScore != null){
                
                SoccerGameOver();
            }
                
        }
        if (TypeOfGame == "Survival Game")
        {
            deathCount = FindObjectOfType<DeathCount> ();
            if (deathCount != null){
                Winner = deathCount.gameObject;
                WinnerPos = Winner.transform.position;
                Distance = Vector3.Distance(WinnerPos , Camera.transform.position);
                Radius = Vector2.Distance(new Vector2(WinnerPos.x,WinnerPos.z),new Vector2(Camera.transform.position.x,Camera.transform.position.z));
                StartCoroutine(GetClose());
                SurvivalGameOver();
            }
                
        }
        gameoverCanvas.enabled = true;    // 把結束Canvas打開
    }

    //-------
    private IEnumerator GetClose()
    {
        Debug.Log(Radius);
        while (Radius >= CameraDis)
        {
            Camera.transform.Translate(Vector3.Normalize(WinnerPos+(new Vector3(0,5,0)) - Camera.transform.position)*0.5f);
            Camera.transform.LookAt(WinnerPos);
            yield return 0;
        }
        Angle = Mathf.Acos((Camera.transform.position.x - WinnerPos.x)/Radius);
        Camera.transform.rotation = Quaternion.LookRotation(WinnerPos);
        StartCoroutine(Rotate());
        yield return 0;
    }
    private IEnumerator Rotate(){
        while(true){
            Angle += Time.deltaTime*RotateSpeed;
            Camera.transform.position = new Vector3(Mathf.Cos(-Angle)*Radius + WinnerPos.x , Camera.transform.position.y , Mathf.Sin(-Angle)*Radius + WinnerPos.z);
            Camera.transform.LookAt(WinnerPos);
            yield return 0;
        }
    }
    //-------

    void SurvivalGameOver()
    {
        string winnerName = deathCount.gameObject.name;
        resultText.text = winnerName + " Win!";
    }

    void SoccerGameOver()
    {
        /* 依照分數決定印出的結果 */
        float team1Score = soccerScore.team1Score;
        float team2Score = soccerScore.team2Score;
        int numPlayers = FindObjectOfType<GameSetting>().numPlayers;
        Directlight.SetActive(false);
        if (team1Score > team2Score)
        {
            resultText.text = "Team 1 Win!";
            Spotlights.transform.GetChild(0).gameObject.SetActive(true);
            if(numPlayers==4){
                Spotlights.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        else if (team1Score < team2Score)
        {
            resultText.text = "Team 2 Win!";
            if(numPlayers==4){
                Spotlights.transform.GetChild(2).gameObject.SetActive(true);
                Spotlights.transform.GetChild(3).gameObject.SetActive(true);
            }
            else if(numPlayers==2){
                Spotlights.transform.GetChild(1).gameObject.SetActive(true);
            }
            
        }
        else
        {
            resultText.text = "Tied Game";
        }
    }
}
