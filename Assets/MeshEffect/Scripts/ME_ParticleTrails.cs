using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public class ME_ParticleTrails : MonoBehaviour
{
    public GameObject TrailPrefab;

    private ParticleSystem ps;
    ParticleSystem.Particle[] particles;

    private Dictionary<uint, GameObject> hashTrails = new Dictionary<uint, GameObject>();
    private Dictionary<uint, GameObject> newHashTrails = new Dictionary<uint, GameObject>();
    private List<GameObject> currentGO = new List<GameObject>();
    

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    void OnEnable()
    {
        InvokeRepeating("ClearEmptyHashes", 1, 1);
    }

    void OnDisable()
    {
        Clear();
        CancelInvoke("ClearEmptyHashes");
    }


    public void Clear()
    {
       foreach (var go in currentGO)
        {
            Destroy(go);
        }
        currentGO.Clear();
    }

    void Update()
    {
        UpdateTrail();
    }

    void UpdateTrail()
    {

        newHashTrails.Clear();
        int count = ps.GetParticles(particles);
       
        for (int i = 0; i < count; i++)
        {
            if (!hashTrails.ContainsKey(particles[i].randomSeed))
            {
                var go = Instantiate(TrailPrefab, transform.position, new Quaternion());
                go.transform.parent = transform;
                currentGO.Add(go);
                //go.hideFlags = HideFlags.HideInHierarchy;
                newHashTrails.Add(particles[i].randomSeed, go);
                var trail = go.GetComponent<LineRenderer>();
                trail.widthMultiplier *= particles[i].startSize;
            }
            else
            {
                var go = hashTrails[particles[i].randomSeed];
                if (go != null)
                {
                    var trail = go.GetComponent<LineRenderer>();
                    trail.startColor *= particles[i].GetCurrentColor(ps);
                    trail.endColor *= particles[i].GetCurrentColor(ps);

                    if (ps.main.simulationSpace == ParticleSystemSimulationSpace.World)
                        go.transform.position = particles[i].position;
                    if (ps.main.simulationSpace == ParticleSystemSimulationSpace.Local)
                        go.transform.position = ps.transform.TransformPoint(particles[i].position);

                    newHashTrails.Add(particles[i].randomSeed, go);
                }
                
                hashTrails.Remove(particles[i].randomSeed);
            }
        }

        foreach (var hashTrail in hashTrails)
        {
            if (hashTrail.Value != null) hashTrail.Value.GetComponent<ME_TrailRendererNoise>().IsActive = false;
        }

        AddRange(hashTrails, newHashTrails);

    }

    public void AddRange<T, S>(Dictionary<T, S> source, Dictionary<T, S> collection)
    {
        if (collection == null)
        {
            return;
        }

        foreach (var item in collection)
        {
            if (!source.ContainsKey(item.Key))
            {
                source.Add(item.Key, item.Value);
            }
        }
    }

    void ClearEmptyHashes()
    {
        hashTrails = hashTrails.Where(h => h.Value != null).ToDictionary(h => h.Key, h => h.Value);
    }
}
