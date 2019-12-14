using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Magnetic : MonoBehaviour
{
    public Sprite NorthPoleMat;       // N極圖案
    public Sprite SouthPoleMat;       // S極圖案
    public Image MagneticImage;       // 顯示磁極的Image
    int magnetPole = 1;               // 磁極
    Attractor attractor;

    void Start()
    {
        MagneticImage.sprite = NorthPoleMat;
        attractor = GetComponent<Attractor> ();
        attractor.magnetPole = magnetPole;              // 初始值為N極
    }

    /* 切換磁極 */
    public void ToggleMagnetPole ()
    {
        magnetPole = -magnetPole;
        attractor.magnetPole = magnetPole;
        if (magnetPole == 1)
        {
            MagneticImage.sprite = NorthPoleMat;
        }
        if (magnetPole == -1)
        {
            MagneticImage.sprite = SouthPoleMat;
        }
    }

}
