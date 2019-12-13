using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEject : MonoBehaviour
{
    public float upForceMag = 5.0f;    // 向上彈射力量
    List<GameObject> Balls;

    void FixedUpdate()
    {
        if (Balls != null)
        {
            /* 若目前有玩家接觸到機關, 就把他們向上彈飛 */
            foreach (GameObject ball in Balls)
            {
                Rigidbody rb = ball.GetComponent<Rigidbody> ();
                Vector3 upForce = upForceMag * Vector3.up;
                rb.AddForce(upForce);
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        /* 若接觸到機關, 就把該玩家添加至List */
        if (Balls == null)
            Balls = new List<GameObject> ();

        Balls.Add(col.gameObject);
    }

    void OnCollisionExit(Collision col)
    {
        /* 若離開機關, 就把該玩家從List刪除 */
        Balls.Remove(col.gameObject);
    }
}
