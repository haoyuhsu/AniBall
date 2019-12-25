using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public ParticleSystem ringParticle;     // 圓環粒子效果 (玩家代表色)
    public ParticleSystem rainbowParticle;  // 彩虹圓環 (當玩家吃到道具的時候)
    public GameObject bottom;               // 玩家底部的GameObject, 將圓環放到它Hierarchy底下
    bool onFloor = false;                   // 是否接觸到地板
    Quaternion orig_rotation;
    Vector3 orig_position;
    Vector3 orig_offset;

    void Start()
    {
        ringParticle = Instantiate(ringParticle, bottom.transform.position, Quaternion.Euler(-90f, 0, 0));
        ringParticle.transform.parent = bottom.transform;

        rainbowParticle = Instantiate(rainbowParticle, bottom.transform.position, Quaternion.Euler(-90f, 0, 0));
        rainbowParticle.transform.parent = bottom.transform;
        rainbowParticle.Stop();

        orig_rotation = bottom.transform.rotation;
        orig_position = bottom.transform.localPosition;
        orig_offset = bottom.transform.position - transform.position;
    }

    void FixedUpdate()
    {
        bottom.transform.rotation = orig_rotation;
        bottom.transform.position = transform.position + orig_offset;
        //bottom.transform.localPosition = orig_position;

        /*if (onFloor)                               // 如果在地上, 則播放圓環粒子效果
        {
            //ringParticle.enableEmission = true;
        }
        else
        {
            //ringParticle.enableEmission = false;
        }*/
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Floor")
        {
            onFloor = true;
        }
    }

    void OnCollisionExit(Collision col)
    {
        onFloor = false;
    }

    public void PlayRainbowParticle()
    {
        rainbowParticle.Play();
    }

    public void StopRainbowParticle()
    {
        rainbowParticle.Stop();
    }

}
