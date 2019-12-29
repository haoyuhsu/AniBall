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
    public ParticleSystem[] Fireworks;
    public MusicController musicController;
    public GameObject Scores;
    public GameObject Teams;

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
                StartCoroutine(Survival_Showcanvas());
            }
        }
    }

    //-------
    IEnumerator Survival_Showcanvas(){
        yield return new WaitForSeconds(2);
        gameoverCanvas.enabled = true;
        Transform Rank;
        Transform Winner = Teams.transform.GetChild(deathCount.playerIndex);
        float Winner_Scale = Winner.transform.localScale.x;
        float Rank_Scale;
        if(deathCount.lifeLeft >= 3){
            Rank = Scores.transform.GetChild(0);
        }
        else if(deathCount.lifeLeft == 2){
            Rank = Scores.transform.GetChild(1);
        }
        else {
            Rank = Scores.transform.GetChild(2);
        }
        Rank_Scale = Rank.transform.localScale.x;
        Winner.transform.localScale = new Vector3(Winner_Scale*25,Winner_Scale*25,0);
        Rank.transform.localScale = new Vector3(Rank_Scale*25,Rank_Scale*25,0);
        Winner.gameObject.SetActive(true);
        while(Winner.transform.localScale.x > Winner_Scale){
            Winner.transform.localScale -= new Vector3(Winner_Scale,Winner_Scale,0);
            yield return 0;
        }
        musicController.PlayRank();
        yield return new WaitForSeconds(0.1f);
        Rank.gameObject.SetActive(true);
        while(Rank.transform.localScale.x > Rank_Scale){
            Rank.transform.localScale -= new Vector3(Rank_Scale,Rank_Scale,0);
            yield return 0;
        }
        musicController.PlayRank();
        yield return 0;
    }
    IEnumerator GetClose()
    {
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
    IEnumerator Rotate(){
        while(true){
            Angle += Time.deltaTime*RotateSpeed;
            Camera.transform.position = new Vector3(Mathf.Cos(-Angle)*Radius + WinnerPos.x , Camera.transform.position.y , Mathf.Sin(-Angle)*Radius + WinnerPos.z);
            Camera.transform.LookAt(WinnerPos);
            yield return 0;
        }
    }
    //-------

    void SoccerGameOver()
    {
        /* 依照分數決定印出的結果 */
        float team1Score = soccerScore.team1Score;
        float team2Score = soccerScore.team2Score;
        int numPlayers = FindObjectOfType<GameSetting>().numPlayers;
        int winner = -1;
        Directlight.SetActive(false);
        Fireworks[0].Play();
        Fireworks[1].Play();
        musicController.PlayGoalClip();
        musicController.PlayRefWhistle();
        if (team1Score > team2Score)
        {
            Spotlights.transform.GetChild(0).gameObject.SetActive(true);
            if(numPlayers==4){
                Spotlights.transform.GetChild(2).gameObject.SetActive(true);
            }
        }
        else if (team1Score < team2Score)
        {
            if(numPlayers==4){
                Spotlights.transform.GetChild(1).gameObject.SetActive(true);
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
        StartCoroutine(Soccer_ShowCanvas(team1Score,team2Score));
    }

    IEnumerator Soccer_ShowCanvas(float team1Score,float team2Score){
        float Score = team1Score - team2Score;
        Transform WinnerTeam;
        Transform Rank;
        yield return new WaitForSeconds(2.5f);
        gameoverCanvas.enabled = true;
        if(Score>0){
            WinnerTeam = Teams.transform.GetChild(0);
            WinnerTeam.gameObject.SetActive(true);
            WinnerTeam.localScale = new Vector3(99,99,0);
            while(WinnerTeam.localScale.x>3){
                WinnerTeam.localScale -= new Vector3(1,1,0);
                yield return 0;
            }
            musicController.PlayRank();
            yield return new WaitForSeconds(0.1f);
            if(Score>=4){
                Rank = Scores.transform.GetChild(0);
            }
            else if(Score==3){
                Rank = Scores.transform.GetChild(1);
            }
            else {
                Rank = Scores.transform.GetChild(2);
            }
            Rank.gameObject.SetActive(true);
            float Scale = Rank.localScale.x;
            Rank.localScale = new Vector3(Scale*25,Scale*25,0);
            while(Rank.localScale.x > Scale){
                Rank.localScale -= new Vector3(Scale/4,Scale/4,0);
                yield return 0;
            }
            musicController.PlayRank();
            yield return 0;
        }
        else if (Score<0){
            WinnerTeam = Teams.transform.GetChild(1);
            WinnerTeam.gameObject.SetActive(true);
            float Winner_Scale = WinnerTeam.localScale.x;
            WinnerTeam.localScale = new Vector3(Winner_Scale*25,Winner_Scale*25,0);
            while(WinnerTeam.localScale.x > Winner_Scale){
                WinnerTeam.localScale -= new Vector3(Winner_Scale,Winner_Scale,0);
                yield return 0;
            }
            musicController.PlayRank();
            yield return new WaitForSeconds(0.1f);
            if(Score<=-4){
                Rank = Scores.transform.GetChild(0);
            }
            else if(Score==-3){
                Rank = Scores.transform.GetChild(1);
            }
            else {
                Rank = Scores.transform.GetChild(2);
            }
            Rank.gameObject.SetActive(true);
            float Rank_Scale = Rank.localScale.x;
            Rank.localScale = new Vector3(Rank_Scale*25,Rank_Scale*25,0);
            while(Rank.localScale.x > Rank_Scale){
                Rank.localScale -= new Vector3(Rank_Scale,Rank_Scale,0);
                yield return 0;
            }
            musicController.PlayRank();
            yield return 0;
        }
    }
}
