using System;
using UnityEngine;

public class ME_DemoGUI : MonoBehaviour
{
    public GameObject Character;
    public GameObject Model;
    public int Current = 0;
    public GameObject[] Prefabs;
   
   
    public Light Sun;
    public ReflectionProbe ReflectionProbe;
    public Light[] NightLights = new Light[0];
    public Texture HUETexture;
    public bool UseMobileVersion;
    public GameObject MobileCharacter;
    public GameObject Target;
    public Color guiColor = Color.red;

    private int currentNomber;
    private GameObject characterInstance;
    private GameObject modelInstance;
    private GUIStyle guiStyleHeader = new GUIStyle();
    private GUIStyle guiStyleHeaderMobile = new GUIStyle();
    float dpiScale;
    private bool isDay;
    private float colorHUE;
    private float startSunIntensity;
    private Quaternion startSunRotation;
    private Color startAmbientLight;
    private float startAmbientIntencity;
    private float startReflectionIntencity;
    private LightShadows startLightShadows;
    //private GameObject mobileCharacterInstance;

    void Start()
    {
        if (Screen.dpi < 1) dpiScale = 1;
        if (Screen.dpi < 200) dpiScale = 1;
        else dpiScale = Screen.dpi/200f;
        guiStyleHeader.fontSize = (int) (15f*dpiScale);
        guiStyleHeader.normal.textColor = guiColor;
        guiStyleHeaderMobile.fontSize = (int) (17f*dpiScale);

        ChangeCurrent(Current);

        startSunIntensity = Sun.intensity;
        startSunRotation = Sun.transform.rotation;
        startAmbientLight = RenderSettings.ambientLight;
        startAmbientIntencity = RenderSettings.ambientIntensity;
        startReflectionIntencity = RenderSettings.reflectionIntensity;
        startLightShadows = Sun.shadows;


    }

    bool isButtonPressed;


    private void OnGUI()
    {
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.DownArrow))
            isButtonPressed = false;

        if (GUI.Button(new Rect(10*dpiScale, 15*dpiScale, 135*dpiScale, 37*dpiScale), "PREVIOUS EFFECT") || (!isButtonPressed && Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            isButtonPressed = true;
            ChangeCurrent(-1);
        }
        if (GUI.Button(new Rect(160*dpiScale, 15*dpiScale, 135*dpiScale, 37*dpiScale), "NEXT EFFECT") || (!isButtonPressed && Input.GetKeyDown(KeyCode.RightArrow)))
        {
            isButtonPressed = true;
            ChangeCurrent(+1);
        }
        var offset = 0f;
        //if (UseMobileVersion)
        //{

        //    offset += 50 * dpiScale;
        //    if (GUI.Button(new Rect(10*dpiScale, 63 * dpiScale, 285*dpiScale, 37*dpiScale), "ON / OFF REALISTIC BLOOM") ||
        //        (!isButtonPressed && Input.GetKeyDown(KeyCode.DownArrow)))
        //    {
        //        isUsedMobileBloom = !isUsedMobileBloom;
        //        RFX1_DistortionAndBloom.UseBloom = isUsedMobileBloom;
        //    }
        //    if(!isUsedMobileBloom) guiStyleHeaderMobile.normal.textColor = new Color(0.8f, 0.2f, 0.2f);
        //    else guiStyleHeaderMobile.normal.textColor = new Color(0.1f, 0.6f, 0.1f);
        //    GUI.Label(new Rect(400 * dpiScale, 15 * dpiScale, 100 * dpiScale, 20 * dpiScale), "Bloom is "+ (isUsedMobileBloom?"Enabled":"Disabled"), guiStyleHeaderMobile);

        //}
        if (GUI.Button(new Rect(10*dpiScale, 63*dpiScale + offset, 285*dpiScale, 37*dpiScale), "Day / Night") || (!isButtonPressed && Input.GetKeyDown(KeyCode.DownArrow)))
        {
            isButtonPressed = true;
            if (ReflectionProbe != null) ReflectionProbe.RenderProbe();
            Sun.intensity = !isDay ? 0.05f : startSunIntensity;
            Sun.shadows = isDay ? startLightShadows : LightShadows.None;
            foreach (var nightLight in NightLights)
            {
                nightLight.shadows = !isDay ? startLightShadows : LightShadows.None;
            }
            Sun.transform.rotation = isDay ? startSunRotation : Quaternion.Euler(350, 30, 90);
            RenderSettings.ambientLight = !isDay ? new Color(0.2f, 0.2f, 0.2f) : startAmbientLight;
            var lightInten = !UseMobileVersion ? 1 : 0.3f;
            RenderSettings.ambientIntensity = isDay ? startAmbientIntencity : lightInten;
            RenderSettings.reflectionIntensity = isDay ? startReflectionIntencity : 0.2f;
            isDay = !isDay;
        }

        GUI.Label(new Rect(400*dpiScale, 15*dpiScale + offset/2, 100*dpiScale, 20*dpiScale),
            "Prefab name is \"" + Prefabs[currentNomber].name + "\"  \r\nHold any mouse button that would move the camera", guiStyleHeader);



        GUI.DrawTexture(new Rect(12*dpiScale, 140*dpiScale + offset, 285*dpiScale, 15*dpiScale), HUETexture, ScaleMode.StretchToFill, false, 0);

        float oldColorHUE = colorHUE;
        colorHUE = GUI.HorizontalSlider(new Rect(12*dpiScale, 147*dpiScale + offset, 285*dpiScale, 15*dpiScale), colorHUE, 0, 360);


        if (Mathf.Abs(oldColorHUE - colorHUE) > 0.001)
        {

            //var animator = characterInstance.GetComponent<ME_AnimatorEvents>();
            //if (animator != null)
            //{
            //    animator.UpdateColor(colorHUE / 360f);
            //}
            var meshUpdater = characterInstance.GetComponentInChildren<PSMeshRendererUpdater>();
            if (meshUpdater != null)
            {
                meshUpdater.UpdateColor(colorHUE / 360f);
            }

            meshUpdater = modelInstance.GetComponentInChildren<PSMeshRendererUpdater>();
            if (meshUpdater != null)
            {
                meshUpdater.UpdateColor(colorHUE / 360f);
            }

            //if (UseMobileVersion)
            //{
            //    var settingColor = currentInstance.GetComponent<RFX1_EffectSettingColor>();
            //    if (settingColor == null) settingColor = currentInstance.AddComponent<RFX1_EffectSettingColor>();
            //    var hsv = RFX1_ColorHelper.ColorToHSV(settingColor.Color);
            //    hsv.H = colorHUE / 360f;
            //    settingColor.Color = RFX1_ColorHelper.HSVToColor(hsv);
            //}


        }
    }

   

    private GameObject instanceShieldProjectile;

    void ChangeCurrent(int delta)
    {
        currentNomber += delta;
        if (currentNomber > Prefabs.Length - 1)
            currentNomber = 0;
        else if (currentNomber < 0)
            currentNomber = Prefabs.Length - 1;

        if (characterInstance != null)
        {
            Destroy(characterInstance);
            RemoveClones();
        }

        if (modelInstance != null)
        {
            Destroy(modelInstance);
            RemoveClones();
        }

        //currentInstance = Instantiate(Prefabs[currentNomber]);
        characterInstance = Instantiate(Character);
        characterInstance.GetComponent<ME_AnimatorEvents>().EffectPrefab = Prefabs[currentNomber];

        modelInstance = Instantiate(Model);
        var effectInstance = Instantiate(Prefabs[currentNomber]);
        effectInstance.transform.parent = modelInstance.transform;
        effectInstance.transform.localPosition = Vector3.zero;
        effectInstance.transform.localRotation = new Quaternion();
        var meshUpdater = effectInstance.GetComponent<PSMeshRendererUpdater>();
        meshUpdater.UpdateMeshEffect(modelInstance);

        //for (int i = 1; i < 3; i++)
        //    for (int j = 1; j < 6; j++)
        //    {
        //        var characterInstanceChild = Instantiate(Model);
        //        characterInstanceChild.transform.parent = modelInstance.transform;
        //        characterInstanceChild.transform.localPosition += new Vector3(-j/2f, i - 0.5f, -j*0.8f);

        //        effectInstance = Instantiate(Prefabs[currentNomber]);
        //        effectInstance.transform.parent = characterInstanceChild.transform;
        //        effectInstance.transform.localPosition = Vector3.zero;
        //        effectInstance.transform.localRotation = new Quaternion();

        //        meshUpdater = effectInstance.GetComponent<PSMeshRendererUpdater>();
        //        meshUpdater.UpdateMeshEffect(characterInstanceChild);
        //    }
        if (UseMobileVersion)
        {
            CancelInvoke("ReactivateEffect");


        }
        //if (mobileCharacterInstance != null)
        //{
        //    Destroy(mobileCharacterInstance);
        //}
    }



    void RemoveClones()
    {
        var allGO = FindObjectsOfType<GameObject>();
        foreach (var go in allGO)
        {
            if (go.name.Contains("(Clone)")) Destroy(go);
        }
    }


    void ReactivateEffect()
    {
        characterInstance.SetActive(false);
        characterInstance.SetActive(true);

        modelInstance.SetActive(false);
        modelInstance.SetActive(true);
    }
}
