using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AimSliderControl : MonoBehaviour
{
    public GameObject Player;    // 玩家物件
    BallMove ballMove;
    public Slider aimSlider;     // Slider物件
    Canvas sliderCanvas;

    void Start()
    {
        ballMove = Player.GetComponent<BallMove> ();
        sliderCanvas = GetComponent<Canvas> ();
    }

    void Update()
    {
        aimSlider.value = ballMove.launchForce;   // 蓄力力量值轉為Slider長度
        /* 如果目前是蓄力狀態, 就顯示slider */
        if (ballMove.OnReady)
        {
            sliderCanvas.enabled = true;
        }
        else
        {
            sliderCanvas.enabled = false;
        }
    }
}
