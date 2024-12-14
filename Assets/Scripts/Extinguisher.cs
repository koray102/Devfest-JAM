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
    private List<Rigidbody> objects = new List<Rigidbody>();
    private Rigidbody playerRb;
    private bool isShooting;

    void Start()
    {
        playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            isShooting = true;
        }else
        {
            isShooting = false;
        }

        if(isShooting)
        {
            capacity -= Time.deltaTime;
            if(capacitySlider != null)
            {
                capacitySlider.value = capacity / 100f;
            }
        }
    }


    private void FixedUpdate()
    {
        if(isShooting)
        {
            Vector3 dampDirecton = -cam.transform.forward;

            playerRb.AddForce(dampDirecton * pushForce);

            foreach (Rigidbody pushRb in objects)
            {
                if(pushRb != null)
                {
                    Vector3 pushDirection = pushRb.transform.position - transform.position;
                    pushDirection.Normalize();
                    
                    pushRb.AddForce(pushForce * pushDirection);
                }else
                {
                    objects.Remove(pushRb);
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Rigidbody enteredRigidbody = other.gameObject.GetComponent<Rigidbody>();
        
        if(enteredRigidbody != null && !enteredRigidbody.isKinematic && !other.gameObject.isStatic && other.gameObject.layer != ignoredLayers)
        {
            objects.Add(enteredRigidbody);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        Rigidbody exitRigidbody = other.gameObject.GetComponent<Rigidbody>();

        if(exitRigidbody != null && objects.Contains(exitRigidbody))
        {
            objects.Remove(exitRigidbody);
        }
    }
}
