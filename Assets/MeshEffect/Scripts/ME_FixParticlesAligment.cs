using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ME_FixParticlesAligment : MonoBehaviour
{
#if UNITY_2017_1_OR_NEWER
    
    // Start is called before the first frame update
    void Start()
    {
        var ps = GetComponent<ParticleSystem>();
        var rend = ps.GetComponent<ParticleSystemRenderer>();
        rend.alignment = ParticleSystemRenderSpace.World;
    }
#endif


}
