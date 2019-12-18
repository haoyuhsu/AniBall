using UnityEngine;

[ExecuteInEditMode]
public class ME_ParticleGravityPoint : MonoBehaviour
{
    public Transform target;
    public float Force = 1;
    public bool DistanceRelative;
    ParticleSystem ps;
    ParticleSystem.Particle[] particles;

    ParticleSystem.MainModule mainModule;
    Vector3 prevPos;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        mainModule = ps.main;
    }

    void LateUpdate()
    {
        var maxParticles = mainModule.maxParticles;
        if (particles == null || particles.Length < maxParticles)
        {
            particles = new ParticleSystem.Particle[maxParticles];
        }
        int particleCount = ps.GetParticles(particles);
       
        var targetTransformedPosition = Vector3.zero;
        if(mainModule.simulationSpace == ParticleSystemSimulationSpace.Local)
            targetTransformedPosition = transform.InverseTransformPoint(target.position);
        if(mainModule.simulationSpace == ParticleSystemSimulationSpace.World)
            targetTransformedPosition = target.position;

        
        float forceDeltaTime = Time.deltaTime * Force;
        if (DistanceRelative)
            forceDeltaTime *= Mathf.Abs((prevPos - targetTransformedPosition).magnitude);

       

        for (int i = 0; i < particleCount; i++)
        {
            var distanceToParticle = targetTransformedPosition - particles[i].position;
            var directionToTarget = Vector3.Normalize(distanceToParticle);
            if (DistanceRelative)
                directionToTarget = Vector3.Normalize(targetTransformedPosition - prevPos);

            var seekForce = directionToTarget * forceDeltaTime;
            
            particles[i].velocity += seekForce;
        }

        ps.SetParticles(particles, particleCount);
        prevPos = targetTransformedPosition;
    }
}
