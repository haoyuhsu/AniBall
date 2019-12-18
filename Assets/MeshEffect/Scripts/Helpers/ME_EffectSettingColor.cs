using UnityEngine;
using System.Collections;

public class ME_EffectSettingColor : MonoBehaviour
{
    public Color Color = Color.red;
    private Color previousColor;

    void OnEnable()
    {
        Update();
    }

    void Update()
    {
        if (previousColor != Color)
        {
            UpdateColor();
        }
    }

    private void UpdateColor()
    {
        var hue = ME_ColorHelper.ColorToHSV(Color).H;
        ME_ColorHelper.ChangeObjectColorByHUE(gameObject, hue);
        previousColor = Color;
    }

}
