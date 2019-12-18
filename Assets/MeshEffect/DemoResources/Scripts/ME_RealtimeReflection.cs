using UnityEngine;
using System.Collections;

public class ME_RealtimeReflection : MonoBehaviour
{
    ReflectionProbe probe;
    private Transform camT;
    public bool CanUpdate = true;

    void Awake()
    {
        probe = GetComponent<ReflectionProbe>();
        camT = Camera.main.transform;
    }

    void Update()
    {
        var pos = camT.position;
        probe.transform.position = new Vector3(
            pos.x,
            pos.y * -1,
            pos.z
        );
        if(CanUpdate) probe.RenderProbe();
    }
}
