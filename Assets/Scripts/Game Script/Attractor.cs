using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    const float G = 6.674f;
    public static List<Attractor> Attractors;
    public Rigidbody rb;
    public bool isBall = false;

    void FixedUpdate()
    {
        if (!isBall)
        {
            /* 若為機關的話, 則對所有Attach Attractor的物件做Attract */
            foreach (Attractor attractor in Attractors)
            {
                if (attractor != this)
                    Attract(attractor);
            }
        }
    }

    void OnEnable ()
    {
        /* 剛啟動時, 把自己物件加入到Attractors Static List */
        if (Attractors == null)
            Attractors = new List<Attractor> ();

        Attractors.Add(this);
    }

    void OnDisable ()
    {
        /* 關閉時, 則把自己的物件從Attractors Static List中移除 */
        Attractors.Remove(this);
    }

    void Attract (Attractor objToAttract)
    {
        Rigidbody rbToAttract = objToAttract.rb;

        Vector3 direction = rb.position - rbToAttract.position;
        float distance = direction.sqrMagnitude;

        if (distance == 0f) return;

        float forceMagnitude = G * (rb.mass * rbToAttract.mass) / distance;
        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }
}
