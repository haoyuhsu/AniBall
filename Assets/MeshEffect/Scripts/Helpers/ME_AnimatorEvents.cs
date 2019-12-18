using UnityEngine;
using System.Collections;

public class ME_AnimatorEvents : MonoBehaviour
{
    //public RFX1_EffectAnimatorProperty Effect1;
    public GameObject EffectPrefab;
    public GameObject SwordPrefab;
    public Transform SwordPosition;
    public Transform StartSwordPosition;

    GameObject EffectInstance;
    GameObject SwordInstance;

    void Start()
    {
        if (SwordInstance != null) Destroy(SwordInstance);
        SwordInstance = Instantiate(SwordPrefab, StartSwordPosition.position, StartSwordPosition.rotation);
        SwordInstance.transform.parent = StartSwordPosition.transform;
    }
    
    public void ActivateEffect()
    {
        if (EffectPrefab == null || SwordInstance == null) return;

        if (EffectInstance != null) Destroy(EffectInstance);
        EffectInstance = Instantiate(EffectPrefab);
        EffectInstance.transform.parent = SwordInstance.transform;
        EffectInstance.transform.localPosition = Vector3.zero;
        EffectInstance.transform.localRotation = new Quaternion();

        var meshUpdater = EffectInstance.GetComponent<PSMeshRendererUpdater>();
        meshUpdater.UpdateMeshEffect(SwordInstance);
    }

    public void ActivateSword()
    {
        SwordInstance.transform.parent = SwordPosition.transform;
        SwordInstance.transform.position = SwordPosition.position;
        SwordInstance.transform.rotation = SwordPosition.rotation;
    }

    public void UpdateColor(float HUE)
    {
        if (EffectInstance == null) return;

        var settingColor = EffectInstance.GetComponent<ME_EffectSettingColor>();
        if (settingColor == null) settingColor = EffectInstance.AddComponent<ME_EffectSettingColor>();
        var hsv = ME_ColorHelper.ColorToHSV(settingColor.Color);
        hsv.H = HUE;
        settingColor.Color = ME_ColorHelper.HSVToColor(hsv);
    }
}
