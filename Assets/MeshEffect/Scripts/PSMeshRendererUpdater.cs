using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class PSMeshRendererUpdater : MonoBehaviour
{
    public GameObject MeshObject;
    public float StartScaleMultiplier = 1;
    public Color Color = Color.black;
    const string materialName = "MeshEffect";
    List<Material[]> rendererMaterials = new List<Material[]>();
    List<Material[]> skinnedMaterials = new List<Material[]>();
    public bool IsActive = true;
    public float FadeTime = 1.5f;

    bool currentActiveStatus;
    private bool needUpdateAlpha;

    Color oldColor = Color.black;
    private float currentAlphaTime;

    string[] colorProperties =
   {
        "_TintColor", "_Color", "_EmissionColor", "_BorderColor", "_ReflectColor", "_RimColor",
        "_MainColor", "_CoreColor", "_FresnelColor"
    };

    void OnEnable()
    {
        alpha = 0;
        prevAlpha = 0;
        IsActive = true;
    }

    float alpha;
    float prevAlpha;
    Dictionary<string, float> startAlphaColors;
    private bool previousActiveStatus;
    bool needUpdate;
    bool needLastUpdate;
    Dictionary<ParticleSystem, ParticleStartInfo> startParticleParameters;

    void Update()
    {
        if (!Application.isPlaying) return;
        if (startAlphaColors == null)
        {
            InitStartAlphaColors();
        }

        if (IsActive && alpha < 1) alpha += Time.deltaTime / FadeTime;
        if (!IsActive && alpha > 0) alpha -= Time.deltaTime / FadeTime;

        if (alpha > 0 && alpha < 1)
        {
            needUpdate = true;
        }
        else
        {
            needUpdate = false;
            alpha = Mathf.Clamp01(alpha);
            if (Mathf.Abs(prevAlpha - alpha) >= Mathf.Epsilon) UpdateVisibleStatus();
        }
        prevAlpha = alpha;

        if (needUpdate) UpdateVisibleStatus();


        if (Color != oldColor)
        {
            oldColor = Color;
            UpdateColor(Color);
        }
    }

    void InitStartAlphaColors()
    {
        startAlphaColors = new Dictionary<string, float>();

        var renderers = GetComponentsInChildren<Renderer>(true);
        foreach (var rend in renderers)
        {
            var mats = rend.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                if(mats[i].name.Contains(materialName))  GetStartAlphaByProperties(rend.GetHashCode().ToString(), i, mats[i]);
            }
        }

        var skinRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach (var rend in skinRenderers)
        {
            var mats = rend.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i].name.Contains(materialName)) GetStartAlphaByProperties(rend.GetHashCode().ToString(), i, mats[i]);
            }
        }

        var lights = GetComponentsInChildren<Light>(true);
        for (int i = 0; i < lights.Length; i++)
        {
            var lightCurve = lights[i].GetComponent<ME_LightCurves>();
            float intencity = 1;
            if (lightCurve != null) intencity = lightCurve.GraphIntensityMultiplier;
            startAlphaColors.Add(lights[i].GetHashCode().ToString() + i, intencity);
        }


        renderers = MeshObject.GetComponentsInChildren<Renderer>(true);
        foreach (var rend in renderers)
        {
            var mats = rend.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i].name.Contains(materialName)) GetStartAlphaByProperties(rend.GetHashCode().ToString(), i, mats[i]);
            }
        }

        skinRenderers = MeshObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach (var rend in skinRenderers)
        {
            var mats = rend.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i].name.Contains(materialName)) GetStartAlphaByProperties(rend.GetHashCode().ToString(), i, mats[i]);
            }
        }
    }

    void InitStartParticleParameters()
    {
        startParticleParameters = new Dictionary<ParticleSystem, ParticleStartInfo>();
        var particles = MeshObject.GetComponentsInChildren<ParticleSystem>(true);
        foreach (var ps in particles)
        {
             startParticleParameters.Add(ps, new ParticleStartInfo {StartSize = ps.main.startSize, StartSpeed = ps.main.startSpeed});
        }
    }

    void UpdateVisibleStatus()
    {
        var renderers = GetComponentsInChildren<Renderer>(true);
        foreach (var rend in renderers)
        {
            var mats = rend.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i].name.Contains(materialName)) UpdateAlphaByProperties(rend.GetHashCode().ToString(), i, mats[i], alpha);
            }
        }

        var skinRenderers = GetComponentsInChildren<Renderer>(true);
        foreach (var rend in skinRenderers)
        {
            var mats = rend.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i].name.Contains(materialName)) UpdateAlphaByProperties(rend.GetHashCode().ToString(), i, mats[i], alpha);
            }
        }

        renderers = MeshObject.GetComponentsInChildren<Renderer>(true);
        foreach (var rend in renderers)
        {
            var mats = rend.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i].name.Contains(materialName)) UpdateAlphaByProperties(rend.GetHashCode().ToString(), i, mats[i], alpha);
            }
        }

        skinRenderers = MeshObject.GetComponentsInChildren<Renderer>(true);
        foreach (var rend in skinRenderers)
        {
            var mats = rend.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i].name.Contains(materialName)) UpdateAlphaByProperties(rend.GetHashCode().ToString(), i, mats[i], alpha);
            }
        }

        var lightCurves = GetComponentsInChildren<ME_LightCurves>(true);
        foreach (var lightCurve in lightCurves)
        {
            lightCurve.enabled = IsActive;
        }

        var lights = GetComponentsInChildren<Light>(true);
        for (int i = 0; i < lights.Length; i++)
        {
            if (!IsActive)
            {
                var startAlpha = startAlphaColors[lights[i].GetHashCode().ToString() + i];
                lights[i].intensity = alpha * startAlpha;
            }
        }
        
        var particleSystems = GetComponentsInChildren<ParticleSystem>(true);
        foreach (var ps in particleSystems)
        {
            if (!IsActive && !ps.isStopped) ps.Stop();
            if (IsActive && ps.isStopped) ps.Play();
        }

        var currentTrails = GetComponentsInChildren<ME_TrailRendererNoise>();
        foreach (var trail in currentTrails)
        {
            trail.IsActive = IsActive;
        }
    }


    void UpdateAlphaByProperties(string rendName, int materialNumber, Material mat, float alpha)
    {
        foreach (var prop in colorProperties)
        {
            if (mat.HasProperty(prop))
            {
                var startAlpha = startAlphaColors[rendName + materialNumber + prop.ToString()];
                var color = mat.GetColor(prop);
                color.a = alpha * startAlpha;
                mat.SetColor(prop, color);
            }
        }
    }

    void GetStartAlphaByProperties(string rendName, int materialNumber, Material mat)
    {
        foreach (var prop in colorProperties)
        {
            if (mat.HasProperty(prop))
            {
                var key = rendName + materialNumber + prop.ToString();
                if (!startAlphaColors.ContainsKey(key))                startAlphaColors.Add(rendName + materialNumber + prop.ToString(), mat.GetColor(prop).a);
            }
        }
    }

    
    public void UpdateColor(Color color)
    {
        if (MeshObject == null) return;
        var hsv = ME_ColorHelper.ColorToHSV(color);
        ME_ColorHelper.ChangeObjectColorByHUE(MeshObject, hsv.H);
    }

    public void UpdateColor(float HUE)
    {
        if (MeshObject == null) return;
        ME_ColorHelper.ChangeObjectColorByHUE(MeshObject, HUE);
    }

    public void UpdateMeshEffect()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = new Quaternion();
        rendererMaterials.Clear();
        skinnedMaterials.Clear();
        if (MeshObject == null) return;
        UpdatePSMesh(MeshObject);
        AddMaterialToMesh(MeshObject);
    }

    void CheckScaleIncludedParticles()
    {
        
    }

    public void UpdateMeshEffect(GameObject go)
    {
        rendererMaterials.Clear();
        skinnedMaterials.Clear();
        if (go == null)
        {
            Debug.Log("You need set a gameObject");
            return;
        }
        MeshObject = go;
        UpdatePSMesh(MeshObject);
        AddMaterialToMesh(MeshObject);
    }

    private void UpdatePSMesh(GameObject go)
    {
        if (startParticleParameters == null) InitStartParticleParameters();

        var ps = GetComponentsInChildren<ParticleSystem>();
        var meshRend = go.GetComponentInChildren<MeshRenderer>();
        var skinMeshRend = go.GetComponentInChildren<SkinnedMeshRenderer>();
        var lights = GetComponentsInChildren<Light>();

        float realBound = 1;
        float transformMax = 1;
        if (meshRend != null)
        {
            realBound = meshRend.bounds.size.magnitude;
            transformMax = meshRend.transform.lossyScale.magnitude;
        }
        if (skinMeshRend != null)
        {
            realBound = skinMeshRend.bounds.size.magnitude;
            transformMax = skinMeshRend.transform.lossyScale.magnitude;
        }

       
        foreach (var particleSys in ps)
        {
            particleSys.transform.gameObject.SetActive(false);
            var sh = particleSys.shape;
            if (sh.enabled)
            {
                if (meshRend != null)
                {
                    sh.shapeType = ParticleSystemShapeType.MeshRenderer;
                    sh.meshRenderer = meshRend;
                    
                }
                if (skinMeshRend != null)
                {
                    sh.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
                    sh.skinnedMeshRenderer = skinMeshRend;
                   
                }
            }

            var mainPS = particleSys.main;
            var startParticleInfo = startParticleParameters[particleSys];
            mainPS.startSize = UpdateParticleParam(startParticleInfo.StartSize, mainPS.startSize, (realBound / transformMax) * StartScaleMultiplier);
            mainPS.startSpeed = UpdateParticleParam(startParticleInfo.StartSpeed, mainPS.startSpeed, (realBound / transformMax) * StartScaleMultiplier);
           
            //var startSize = mainPS.startSize;
            //if (startSize.mode == ParticleSystemCurveMode.TwoConstants)
            //{
            //    startSize.constantMin *= realBound / transformMax;
            //    startSize.constantMax *= realBound / transformMax;
            //}
            //if (startSize.mode == ParticleSystemCurveMode.Constant)
            //    startSize.constant *= realBound / transformMax;
            //else mainPS.startSizeMultiplier *= realBound / transformMax;
            //mainPS.startSize = startSize;



            particleSys.transform.gameObject.SetActive(true);
        }
        if (meshRend != null) foreach (var light1 in lights) light1.transform.position = meshRend.bounds.center;
        if (skinMeshRend != null) foreach (var light1 in lights) light1.transform.position = skinMeshRend.bounds.center;

    }

    class ParticleStartInfo
    {
        public ParticleSystem.MinMaxCurve StartSize;
        public ParticleSystem.MinMaxCurve StartSpeed;
    }

    ParticleSystem.MinMaxCurve UpdateParticleParam(ParticleSystem.MinMaxCurve startParam, ParticleSystem.MinMaxCurve currentParam, float scale)
    {
        if (currentParam.mode == ParticleSystemCurveMode.TwoConstants)
        {
            
            currentParam.constantMin = startParam.constantMin * scale;
            currentParam.constantMax = startParam.constantMax * scale;
        }
        else if (currentParam.mode == ParticleSystemCurveMode.Constant)
            currentParam.constant = startParam.constant * scale;
        return currentParam;
    }

    private void AddMaterialToMesh(GameObject go)
    {
        var meshMatEffect = GetComponentInChildren<ME_MeshMaterialEffect>();
        if (meshMatEffect == null) return;

        var meshRenderer = go.GetComponentInChildren<MeshRenderer>();
        var skinMeshRenderer = go.GetComponentInChildren<SkinnedMeshRenderer>();

        // foreach (var meshRenderer in meshRenderers)
        if (meshRenderer != null)
        {
            rendererMaterials.Add(meshRenderer.sharedMaterials);
            meshRenderer.sharedMaterials = AddToSharedMaterial(meshRenderer.sharedMaterials, meshMatEffect);
        }

       // foreach (var skinMeshRenderer in skinMeshRenderers)
        if(skinMeshRenderer!=null)
        {
            skinnedMaterials.Add(skinMeshRenderer.sharedMaterials);
            skinMeshRenderer.sharedMaterials = AddToSharedMaterial(skinMeshRenderer.sharedMaterials, meshMatEffect);
        }
    }

    Material[] AddToSharedMaterial(Material[] sharedMaterials, ME_MeshMaterialEffect meshMatEffect)
    {
        if (meshMatEffect.IsFirstMaterial) return new[] { meshMatEffect.Material };
        var materials = sharedMaterials.ToList();
        for (int i = 0; i < materials.Count; i++)
        {
            if (materials[i].name.Contains(materialName)) materials.RemoveAt(i);
        }
        //meshMatEffect.Material.name = meshMatEffect.Material.name + materialName;
        materials.Add(meshMatEffect.Material);
        return materials.ToArray();
    }

    void OnDestroy()
    {
        //Activation(true);
        if (MeshObject == null) return;
        var meshRenderers = MeshObject.GetComponentsInChildren<MeshRenderer>();
        var skinMeshRenderers = MeshObject.GetComponentsInChildren<SkinnedMeshRenderer>();

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            if (rendererMaterials.Count == meshRenderers.Length)
                meshRenderers[i].sharedMaterials = rendererMaterials[i];

            var materials = meshRenderers[i].sharedMaterials.ToList();
            for (int j = 0; j < materials.Count; j++)
            {
                if (materials[j].name.Contains(materialName)) materials.RemoveAt(j);
            }
            meshRenderers[i].sharedMaterials = materials.ToArray();

        }

        for (int i = 0; i < skinMeshRenderers.Length; i++)
        {
            if (skinnedMaterials.Count == skinMeshRenderers.Length)
                skinMeshRenderers[i].sharedMaterials = skinnedMaterials[i];

            var materials = skinMeshRenderers[i].sharedMaterials.ToList();
            for (int j = 0; j < materials.Count; j++)
            {
                if (materials[j].name.Contains(materialName)) materials.RemoveAt(j);
            }
            skinMeshRenderers[i].sharedMaterials = materials.ToArray();

        }
        rendererMaterials.Clear();
        skinnedMaterials.Clear();
    }
}