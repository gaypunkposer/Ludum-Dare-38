using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOrbit : MonoBehaviour {

    public float SurfaceOffset;

    ParticleSystem system;
    ParticleSystem.Particle[] particles;

	void Start () {
        system = GetComponent<ParticleSystem>();
	}
	
	// Yes, I seriously do a raycast on each perticle. I'm a madlad.
	void LateUpdate () {
        particles = new ParticleSystem.Particle[system.main.maxParticles];
        int numParticles = system.GetParticles(particles);

        for (int i = 0; i < numParticles; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(particles[i].position, -transform.up, out hit))
            {
                particles[i].position = (hit.point + transform.up * SurfaceOffset);
                //particles[i].rotation3D = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal).eulerAngles;
            }
        }

        system.SetParticles(particles, particles.Length);
	}
}
