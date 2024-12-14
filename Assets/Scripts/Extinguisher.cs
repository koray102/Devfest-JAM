using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Extinguisher : MonoBehaviour
{
    [SerializeField] private float pushForce = 10f;
    [SerializeField] private GameObject cam;
    [SerializeField] private float capacity = 100f;
    [SerializeField] private Slider capacitySlider;
    [SerializeField] private LayerMask ignoredLayers;
    [SerializeField] private ParticleSystem particleEffect; // Reference to the ParticleSystem
    private List<Rigidbody> objects = new List<Rigidbody>();
    private Rigidbody playerRb;
    private bool isShooting;

    private sfxManager SfxManager;

    void Start()
    {
        SfxManager = GameObject.FindGameObjectWithTag("SFX").GetComponent<sfxManager>();
        playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        if (particleEffect == null)
        {
            Debug.LogWarning("Particle System not assigned in the inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) // Left mouse button held down
        {
            
            if (particleEffect != null && !isShooting) // Play particle effect if it's not already playing
            {
                if (!SfxManager.IsSFXPlaying("foamSound", gameObject))
                {
                    SfxManager.PlaySFXOnLoop("foamSound", gameObject);
                }
                particleEffect.Play();
                Debug.Log("PÜSKÜR");
                isShooting = true;
            }
            
        }
        else
        {
            
            if (particleEffect != null && isShooting) // Stop particle effect if the button is released
            {
                if (SfxManager.IsSFXPlaying("foamSound", gameObject))
                {
                    SfxManager.StopSFX("foamSound", gameObject);
                }
                particleEffect.Stop();
                Debug.Log("Dur");
                isShooting = false;
            }
            
        }

        if (isShooting)
        {
            capacity -= Time.deltaTime;
            if (capacitySlider != null)
            {
                capacitySlider.value = capacity / 100f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isShooting)
        {
            
            Vector3 dampDirection = -cam.transform.forward;
            playerRb.AddForce(dampDirection * pushForce);

            foreach (Rigidbody pushRb in objects)
            {
                if (pushRb != null)
                {
                    Vector3 pushDirection = pushRb.transform.position - transform.position;
                    pushDirection.Normalize();

                    pushRb.AddForce(pushForce * pushDirection);
                }
                else
                {
                    objects.Remove(pushRb);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody enteredRigidbody = other.gameObject.GetComponent<Rigidbody>();

        if (enteredRigidbody != null && !enteredRigidbody.isKinematic && !other.gameObject.isStatic && other.gameObject.layer != ignoredLayers)
        {
            objects.Add(enteredRigidbody);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody exitRigidbody = other.gameObject.GetComponent<Rigidbody>();

        if (exitRigidbody != null && objects.Contains(exitRigidbody))
        {
            objects.Remove(exitRigidbody);
        }
    }
}
