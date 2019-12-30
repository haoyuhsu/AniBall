using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Canvas gameoverCanvas;
    public Text resultText;
    public Score soccerScore;
    public bool Ended;
    DeathCount deathCount;

    GameObject Winner = null;
    public GameObject Camera;
    public float RotateSpeed;
    public float CameraDis;
    public GameObject Spotlights;
    public GameObject Directlight;
    Vector3 WinnerPos;
    float Distance;
    float Radius;
    public ParticleSystem[] Fireworks;
    public MusicController musicController;
    public GameObject Scores;
    public GameObject Teams;
    public GameObject Tie;
    public GameObject Background;
    public GameObject PauseButton;
    public float Speed;

    void Start()
    {
        Ended = false;
        gameoverCanvas.enabled = false;     // 初始時把結束Canvas關閉
    }

    void FixedUpdate(){
        if(Winner!=null){
            WinnerPos = Winner.transform.position;
            Distance = Vector3.Distance(WinnerPos , Camera.transform.position);
            Radius = Vector2.Distance(new Vector2(WinnerPos.x,WinnerPos.z),new Vector2(Camera.transform.position.x,Camera.transform.position.z));
        }
    }

    public void OpenGameOverCanvas(string TypeOfGame)
    {
        Ended = true;
        PauseButton.SetActive(false);
        if (TypeOfGame == "Soccer Game")
        {
            if (soccerScore != null){
                float team1Score = soccerScore.team1Score;
                float team2Score = soccerScore.team2Score;
                int numPlayers = FindObjectOfType<GameSetting>().numPlayers;
                int winner = -1;
                Directlight.SetActive(false);
                if (team1Score > team2Score)
                {
                    Fireworks[0].Play();
                    Fireworks[1].Play();
                    musicController.PlayGoalClip();
                    musicController.PlayRefWhistle();
                    Spotlights.transform.GetChild(0).gameObject.SetActive(true);
                    if(numPlayers==4){
                        Spotlights.transform.GetChild(2).gameObject.SetActive(true);
                    }
                }
                else if (team1Score < team2Score)
                {
                    Fireworks[0].Play();
                    Fireworks[1].Play();
                    musicController.PlayGoalClip();
                    musicController.PlayRefWhistle();
                    if(numPlayers==4){
                        Spotlights.transform.GetChild(1).gameObject.SetActive(true);
                        Spotlights.transform.GetChild(3).gameObject.SetActive(true);
                    }
                    else if(numPlayers==2){
                        Spotlights.transform.GetChild(1).gameObject.SetActive(true);
                    }
                }
                StartCoroutine(Soccer_ShowCanvas(team1Score,team2Score));
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
                StartCoroutine(Survival_Showcanvas());
            }
        }
    }

    IEnumerator Soccer_ShowCanvas(float team1Score,float team2Score){
        float Score = team1Score - team2Score;
        yield return new WaitForSecondsRealtime(2);
        gameoverCanvas.enabled = true;
        Time.timeScale = 0;
        
        if(Score == 0) {
            Background.SetActive(false);
            StartCoroutine(Print(Tie.transform));
        }
        else{
            Transform WinnerTeam;
            Transform Rank;
            if(Score>0)
                WinnerTeam = Teams.transform.GetChild(0);
            else 
                WinnerTeam = Teams.transform.GetChild(1);
            if(Mathf.Abs(Score)>=4)
                Rank = Scores.transform.GetChild(0);
            else if(Mathf.Abs(Score)==3)
                Rank = Scores.transform.GetChild(1);
            else 
                Rank = Scores.transform.GetChild(2);
            //Coroutine c1 = 
            yield return StartCoroutine(Print(WinnerTeam));
            yield return new WaitForSecondsRealtime(0.3f);
            StartCoroutine(Print(Rank));
        }
    }

    IEnumerator Survival_Showcanvas(){
        yield return new WaitForSecondsRealtime(2);
        gameoverCanvas.enabled = true;
        Transform Rank;
        Transform Winner = Teams.transform.GetChild(deathCount.playerIndex);
        if(deathCount.lifeLeft >= 3)
            Rank = Scores.transform.GetChild(0);
        else if(deathCount.lifeLeft == 2)
            Rank = Scores.transform.GetChild(1);
        else
            Rank = Scores.transform.GetChild(2);

        yield return StartCoroutine(Print(Winner));
        yield return new WaitForSecondsRealtime(0.3f);
        StartCoroutine(Print(Rank));
    }

    IEnumerator Print(Transform Trans){
        float Scale = Trans.localScale.x;
        Trans.gameObject.SetActive(true);
        Trans.localScale = new Vector3(Scale*Speed,Scale*Speed,0);
        while(Trans.localScale.x > Scale){
            Trans.localScale -= new Vector3(Scale,Scale,0);
            yield return new WaitForEndOfFrame();
        }
        musicController.PlayRank();
        yield return 0;
    }

    IEnumerator GetClose()
    {
        float Angle;
        while (Radius >= CameraDis)
        {
            Camera.transform.Translate(Vector3.Normalize(WinnerPos+(new Vector3(0,5,0)) - Camera.transform.position)*0.5f);
            Camera.transform.LookAt(WinnerPos);
            yield return 0;
        }
        Angle = Mathf.Acos((Camera.transform.position.x - WinnerPos.x)/Radius);
        Camera.transform.rotation = Quaternion.LookRotation(WinnerPos);
        while(true){
            Angle += Time.deltaTime*RotateSpeed;
            Camera.transform.position = new Vector3(Mathf.Cos(-Angle)*Radius + WinnerPos.x , Camera.transform.position.y , Mathf.Sin(-Angle)*Radius + WinnerPos.z);
            Camera.transform.LookAt(WinnerPos);
            yield return 0;
        }
        yield return 0;
    }
}
