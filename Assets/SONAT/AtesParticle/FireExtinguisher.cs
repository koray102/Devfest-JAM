using Unity.VisualScripting;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    public ParticleSystem fireParticleSystem;
    public ParticleSystem lastPuff;
    public float can = 100; // Fire extinguisher fluid amount

    internal bool YokOlcak = false; // Fire extinguished status

    private sfxManager SfxManager;

    private void Start()
    {
        SfxManager = GameObject.FindGameObjectWithTag("SFX").GetComponent<sfxManager>();
    }

    private void Update()
    {
        if (!YokOlcak)
        {
            // Ensure fire sound is playing if fire is still active
            if (!SfxManager.IsSFXPlaying("fireSound", gameObject))
            {
                SfxManager.PlaySFXOnLoop("fireSound", gameObject);
            }
        }
        else
        {
            // Stop fire sound if fire is extinguished
            SfxManager.StopSFX("fireSound", gameObject);
        }
    }

    void OnParticleCollision(GameObject other)
    {
        // Check if the other particle is tagged as "Bubble"
        if (other.CompareTag("Bubble"))
        {
            // Get the ParticleSystem component from the colliding particle
            ParticleSystem particleSystem = other.GetComponent<ParticleSystem>();

            if (particleSystem != null)
            {
                // Destroy the particles in the collision area
                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
                int count = particleSystem.GetParticles(particles);

                for (int i = 0; i < count; i++)
                {
                    if (Vector3.Distance(transform.position, particles[i].position) < 0.1f) // Distance check
                    {
                        particles[i].remainingLifetime = 0; // Destroy the particle
                    }
                }

                particleSystem.SetParticles(particles, count);
            }

            // If fire extinguisher fluid is used up and the fire is still active
            if (fireParticleSystem.isPlaying && can <= 0 && !YokOlcak)
            {
                YokOlcak = true; // Fire is extinguished
                fireParticleSystem.Stop(); // Stop the fire particle system
                lastPuff.Play(); // Play the last puff effect
            }
            // If there's still fluid, decrease the can amount
            else if (can > 0)
            {
                can -= 1;
            }
        }
    }
}
