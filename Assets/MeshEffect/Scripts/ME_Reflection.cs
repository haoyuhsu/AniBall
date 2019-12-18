using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ME_Reflection : MonoBehaviour
{
    public RenderTexture tex;
    private ReflectionProbe reflectionProbe;

    private List<Light> dirLight;
    private List<float> lightIntencity;

	// Use this for initialization
	void Awake ()
	{
	    var lights = GameObject.FindObjectsOfType<Light>();
	    dirLight = new List<Light>();
        lightIntencity = new List<float>();
	    foreach (var l in lights)
	        if (l.type == LightType.Directional)
	        {
	            dirLight.Add(l);
                lightIntencity.Add(l.intensity);
	        }
	    

        reflectionProbe = GetComponent<ReflectionProbe>();
	    tex = new RenderTexture(reflectionProbe.resolution, reflectionProbe.resolution, 0);
	    tex.dimension = TextureDimension.Cube;
	    tex.useMipMap = true;
	    Shader.SetGlobalTexture("ME_Reflection", tex);
        reflectionProbe.RenderProbe(tex);
	  
	}
	
	// Update is called once per frame
	void Update ()
	{
	    bool requireUpdate = false;
	    for (var i = 0; i < dirLight.Count; i++)
	    {
	        if (Math.Abs(dirLight[i].intensity - lightIntencity[i]) > 0.001f)
	        {
	            requireUpdate = true;
	            lightIntencity[i] = dirLight[i].intensity;

	        }
	    }

	    if(requireUpdate) reflectionProbe.RenderProbe(tex);
    }
}