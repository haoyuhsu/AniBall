using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffect : MonoBehaviour
{
    public string ballState = "Normal";                   // 玩家目前狀態
    string curState = "Normal";                           // 紀錄curState, 避免isHit情況時狀態混亂
    public float stayTime = 5.0f;                         // 特殊效果維持時間 (未被觸發)
    public float freezeTime = 3.0f;                       // 被凍結的時間
    public float reverseTime = 3.0f;                      // 被控制顛倒的時間
    public float magnetizeTime = 6.0f;                    // 磁化持續時間 (可以吸引、排斥其他玩家)
    Vector4 bombColor = new Color(1.0f, 0, 0, 0.5f);      // 紅色 (可把人彈飛)
    Vector4 gasColor = new Color(0, 1.0f, 0, 0.5f);       // 綠色 (可把人凍結)
    Vector4 hitColor = new Color(0, 0, 1.0f, 0.5f);       // 藍色 (被凍結控制)
    Vector4 magnetColor = new Color(1.0f, 0.6f, 0.05f);   // 格黃色 (磁化模式)
    public GameObject bombPE;                             // bomb狀態粒子效果
    public GameObject gasPE;                              // gas狀態粒子效果
    public GameObject isHitPE;                            // isHit狀態粒子效果
    public GameObject magnetPE;                           // 磁化狀態粒子效果
    public GameObject BurstPE;                            // bomb狀態和人碰撞時的粒子效果
    public GameObject FreezePE;                           // gas狀態和人碰撞時的粒子效果
    GameObject curPE;                                     // 目前生成的粒子效果
    List<GameObject> ListPE = new List<GameObject>();     // 存著所有生成粒子效果的物件     
    ColorSetting colorSetting;
    AudioSource audioSource;
    public AudioClip burstClip;                           // 爆炸音效
    public AudioClip freezeClip;                          // 凍結音效

    void Start()
    {
        colorSetting = GetComponent<ColorSetting> ();
        audioSource = GetComponent<AudioSource> ();
    }

    void FixedUpdate()
    {
        ColorUpdate();
    }

    void ColorUpdate()
    {
        /* 根據各狀態去改變玩家外部的顏色 */
        if (ballState == "Normal")
        {
            colorSetting.ResetColor();
        }
        else if (ballState == "isBomb")
        {
            colorSetting.SetColor(bombColor);
        }
        else if (ballState == "isGas")
        {
            colorSetting.SetColor(gasColor);
        }
        else if (ballState == "isHit")
        {
            colorSetting.SetColor(hitColor);
        }
        else if (ballState == "isMagnet")
        {
            colorSetting.SetColor(magnetColor);
        }
    }

    public void GrenadeTrigger()
    {
        if (ballState == "Normal" || ballState == "isBomb")
        {
            curState = ballState = "isBomb";
            curPE = Instantiate(bombPE, transform.position, Quaternion.identity);        // 生成紅色炫光
            curPE.transform.parent = gameObject.transform;                               // 將生成Prefab放到ListPE的Hierarchy底下
            ListPE.Add(curPE);
            StartCoroutine(CountDown());
        }
    }

    public void GasTrigger()
    {
        if (ballState == "Normal" || ballState == "isGas")
        {
            curState = ballState = "isGas";
            curPE = Instantiate(gasPE, transform.position, Quaternion.identity);         // 生成綠色炫光
            curPE.transform.parent = gameObject.transform;                               // 將生成Prefab放到ListPE的Hierarchy底下
            ListPE.Add(curPE);
            StartCoroutine(CountDown());
        }
    }

    IEnumerator CountDown()
    {
        /* 透過倒數計時去控制特殊效果持續時間, 時間到未被觸發時即會變回Normal */
        yield return new WaitForSeconds(stayTime);
        if (curState == ballState)
        {
            ballState = "Normal";
            ClearParticleEffects();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")   // 和其他玩家碰撞
        {
            Vector3 hitPoint = col.contacts[0].point;
            if (ballState == "isBomb")
            {
                Burst(col);                    // 把人彈飛
                ballState = "Normal";
                ClearParticleEffects();
                curPE = Instantiate(BurstPE, hitPoint, Quaternion.identity);    // 生成衝撞粒子效果
                curPE.transform.parent = gameObject.transform;
                ListPE.Add(curPE);
                audioSource.PlayOneShot(burstClip, 1.0f);      // 播放炸飛音效
            }
            else if (ballState == "isGas")
            {
                // StartCoroutine(Freeze(col));   // 把人凍結
                StartCoroutine(ReverseControl(col));  // 把人控制顛倒
                ballState = "Normal";
                ClearParticleEffects();
                curPE = Instantiate(FreezePE, hitPoint, Quaternion.identity);    // 生成衝撞粒子效果
                curPE.transform.parent = gameObject.transform;
                ListPE.Add(curPE);
                audioSource.PlayOneShot(freezeClip, 1.0f);     // 播放凍結音效
            }
        }
    }

    void Burst (Collision col)
    {
        /* TODO: Play some bomb effect */
        Rigidbody rb = GetComponent<Rigidbody> ();                         // 本身Rigidbody
        Rigidbody rbToBurst = col.gameObject.GetComponent<Rigidbody> ();   // 對方玩家Rigidbody

        float burstMag = 1000.0f;                                // 彈射力量
        Vector3 direction = rbToBurst.position - rb.position;    // 彈射方向
        Vector3 force = direction.normalized * burstMag;         // 彈射向量
        rbToBurst.AddForce(force);
    }

    public void IsHitTrigger()                   // 被擊到時觸發生成炫光
    {
        ballState = "isHit";
        curPE = Instantiate(isHitPE, transform.position, Quaternion.identity);         // 生成藍色炫光
        curPE.transform.parent = gameObject.transform;                                 // 將生成Prefab放到ListPE的Hierarchy底下
        ListPE.Add(curPE);
    }

    IEnumerator Freeze (Collision col)
    {
        BallMove ballMove = col.gameObject.GetComponent<BallMove> ();
        SpecialEffect se = col.gameObject.GetComponent<SpecialEffect> ();

        se.ballState = "isHit";                         // 改變對方玩家狀態
        ballMove.enabled = false;                       // 凍結對方控制
        yield return new WaitForSeconds(freezeTime);
        se.ballState = "Normal";                        // 回復原本狀態
        se.ClearParticleEffects();
        ballMove.enabled = true;                        // 恢復對方控制
    }

    IEnumerator ReverseControl (Collision col)
    {
        BallMove ballMove = col.gameObject.GetComponent<BallMove> ();
        SpecialEffect se = col.gameObject.GetComponent<SpecialEffect> ();

        se.ballState = "isHit";                         // 改變對方玩家狀態
        IsHitTrigger();
        ballMove.ToggleReverse();                       // 混亂對方控制
        yield return new WaitForSeconds(reverseTime);
        se.ballState = "Normal";                        // 回復原本狀態
        ClearParticleEffects();
        ballMove.ToggleReverse();                       // 恢復對方控制
    }

    public void MagnetTrigger()
    {
        if (ballState == "Normal" || ballState == "isMagnet")
        {
            curState = ballState = "isMagnet";
            curPE = Instantiate(magnetPE, transform.position, Quaternion.identity);        // 生成紫色炫光
            curPE.transform.parent = gameObject.transform;                                 // 將生成Prefab放到ListPE的Hierarchy底下
            ListPE.Add(curPE);
            StartCoroutine(Magnetize());
        }
    }

    IEnumerator Magnetize ()
    {
        /* 將isBall設為false, 使得該玩家可以去對其他玩家造成影響 */
        Attractor attractor = GetComponent<Attractor> ();
        Rigidbody rb = GetComponent<Rigidbody> ();

        attractor.isBall = false;
        rb.mass = 10;                    // 增加質量, 增強對其他玩家的磁力影響
        ballState = "isMagnet";
        yield return new WaitForSeconds(magnetizeTime);
        attractor.isBall = true;
        rb.mass = 1;
        ballState = "Normal";
        ClearParticleEffects();
    }

    public void ClearParticleEffects()
    {
        if (ListPE != null)
        {
            foreach(GameObject PE in ListPE)
            {
                Destroy(PE);
            }
        }
    }
}
