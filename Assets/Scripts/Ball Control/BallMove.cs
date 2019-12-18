using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    // 選定操控方法
    public string vertical_axis;
    public string horizontal_axis;
    public KeyCode shoot = KeyCode.Space;

    // 玩家移動的參數設定
    public float moveForce = 30.0f;            // 移動速度
    public float maxScale = 1.4f;              // 最大膨脹幅度
    public bool OnReady = false;               // 是否開始蓄力
    public float launchForce = 0;              // 紀錄發射力量
    public float launchForceIncrement = 0.5f;  // 每Frame蓄力增加幅度
    Rigidbody m_rigid;
    Vector3 Scale;
    float moveVertical;
    float moveHorizontal;
    int moveDir = 1;           // 移動操控正負
    bool isReverse = false;    // 移動操控是否反向
    bool onGround = false;     // 偵測是否有接觸到地面
    bool launch = false;       // 判斷是否可以發射

    void Start()
    {
        m_rigid = this.gameObject.GetComponent<Rigidbody>();
        Scale = this.gameObject.transform.localScale;
    }
    void Update()
    {
        moveVertical = moveDir * Input.GetAxis (vertical_axis);
		moveHorizontal = moveDir * Input.GetAxis (horizontal_axis);

        if(Input.GetKey(shoot) && !OnReady){        // 按下彈射按鍵開始蓄力 
            OnReady = true;
            m_rigid.velocity = new Vector3(0,0,0);
        }

		if(!Input.GetKey(shoot) && OnReady){                    // 放開彈射按鍵彈射出去
            OnReady = false;
            launch = true;
        }

        if (launch) {                                           // 彈射偵測
            this.gameObject.transform.localScale = Scale;
            m_rigid.velocity = launchForce * Vector3.Normalize(Vector3.forward * moveVertical + Vector3.right * moveHorizontal);
            launch = false;
            launchForce = 0;
        }
    }
    void FixedUpdate()
    {   
        if(OnReady){                                                    // 正在蓄力
            if(this.gameObject.transform.localScale.x <= maxScale) {
                this.gameObject.transform.localScale *= 1.01f;   // 球體膨脹
                launchForce += launchForceIncrement;             // 彈射力量增大
            } else {
                launch = true;                             // 蓄力至最大值時直接彈射
            }
        }
        else{                                                           // 正常移動
            Vector3 movement = new Vector3(moveVertical, 0, -moveHorizontal);
            m_rigid.AddTorque(movement * moveForce);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        onGround = true;
    }
    void OnCollisionExit(Collision col)
    {
        onGround = false;
    }

    public void ToggleReverse()
    {
        isReverse = !isReverse;
        if (isReverse)
        {
            moveDir = -1;
        }
        else
        {
            moveDir = 1;
        }
    }
}
