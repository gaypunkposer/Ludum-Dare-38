using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnEnemies : MonoBehaviour
{
    public Wave[] Waves;
    public WaveManager WaveMan;

    int currentWave;
    int currentEnemy;
    ParticleSystem particlesystem;
    List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        particlesystem = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TriggerWave(int wave)
    {
        currentEnemy = 0;
        currentWave = wave;
        int particleCount = Waves[currentWave].Enemies.Length;
        ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[1];
        ParticleSystem.EmissionModule emissions = particlesystem.emission;
        emissions.GetBursts(bursts);
        bursts[0].minCount = (short)particleCount;
        bursts[0].maxCount = (short)particleCount;
        emissions.SetBursts(bursts);

        particlesystem.Play();
    }

    void OnParticleCollision(GameObject other)
    {

        int numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents(particlesystem, other, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            Vector3 pos = collisionEvents[i].intersection;
            Quaternion rot = Quaternion.LookRotation(Vector3.ProjectOnPlane(Vector3.forward, collisionEvents[i].normal), collisionEvents[i].normal);
            WaveMan.AddEnemy(Instantiate(Waves[currentWave].Enemies[currentEnemy], pos, rot));
            currentEnemy++;
        }
    }
}