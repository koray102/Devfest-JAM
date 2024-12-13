using Unity.VisualScripting;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    public ParticleSystem fireParticleSystem;
    public ParticleSystem lastPuff;
    public float can = 100;

    private bool YokOlcak = false;

    void OnParticleCollision(GameObject other)
    {   
        Debug.Log(other.gameObject.tag);
        // Eðer çarpan partikül "Bubble" ise
        if (other.CompareTag("Bubble"))
        {
            // Çarpýþan partikülün ait olduðu Particle System'i al
            ParticleSystem particleSystem = other.GetComponent<ParticleSystem>();

            if (particleSystem != null)
            {
                // Partiküllerin yok edilmesi
                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
                int count = particleSystem.GetParticles(particles);

                for (int i = 0; i < count; i++)
                {
                    // Partikülün çarptýðý objeyi kontrol et
                    if (Vector3.Distance(transform.position, particles[i].position) < 0.1f) // Mesafe kontrolü
                    {
                        particles[i].remainingLifetime = 0; // Partikülü yok et
                    }
                }

                particleSystem.SetParticles(particles, count);
            }



            // Ateþi söndürmek için fireParticleSystem'i durdur
            if (fireParticleSystem.isPlaying && can <= 0 && !YokOlcak)
            {
                YokOlcak = true;
                fireParticleSystem.Stop();
                lastPuff.Play();
                
            }else if( can > 0 )
            {
                can -= 1;
            }
            
        }
    }
}
