using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffect : MonoBehaviour
{
    public string ballState = "Normal";                   // 玩家目前狀態
    string curState = "Normal";                           // 紀錄curState, 避免isHit情況時狀態混亂
    public float stayTime = 3.0f;                         // 特殊效果維持時間 (未被觸發)
    public float freezeTime = 3.0f;                       // 被凍結的時間
    Vector4 bombColor = new Color(1.0f, 0, 0, 0.5f);      // 紅色 (可把人彈飛)
    Vector4 gasColor = new Color(0, 1.0f, 0, 0.5f);       // 綠色 (可把人凍結)
    Vector4 hitColor = new Color(0, 0, 1.0f, 0.5f);       // 藍色 (被凍結控制)
    //Vector4 origColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    ColorSetting colorSetting;

    void Start()
    {
        colorSetting = GetComponent<ColorSetting> ();
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
    }

    public void GrenadeTrigger()
    {
        if (ballState == "Normal" || ballState == "isBomb")
        {
            curState = ballState = "isBomb";
            StartCoroutine(CountDown());
        }
    }

    public void GasTrigger()
    {
        if (ballState == "Normal" || ballState == "isGas")
        {
            curState = ballState = "isGas";
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
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")   // 和其他玩家碰撞
        {
            if (ballState == "isBomb")
            {
                Burst(col);                    // 把人彈飛
                ballState = "Normal";
            }
            else if (ballState == "isGas")
            {
                StartCoroutine(Freeze(col));   // 把人凍結
                ballState = "Normal";
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

    IEnumerator Freeze (Collision col)
    {
        BallMove ballMove = col.gameObject.GetComponent<BallMove> ();
        SpecialEffect se = col.gameObject.GetComponent<SpecialEffect> ();

        se.ballState = "isHit";                         // 改變對方玩家狀態
        ballMove.enabled = false;                       // 凍結對方控制
        yield return new WaitForSeconds(freezeTime);
        se.ballState = "Normal";                        // 回復原本狀態
        ballMove.enabled = true;                        // 恢復對方控制
    }
}
