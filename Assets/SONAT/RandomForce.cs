using UnityEngine;

public class RandomForce : MonoBehaviour
{
    public float forceAmount = 10f; // Kuvvet miktarý
    public float rotationSpeed = 100f; // Rotasyon hýzý

    private Rigidbody rb;
    private bool isStopped = false; // Hareketi ve rotasyonu durdurma kontrolü
    private bool die = false;
    void Start()
    {
        // Rigidbody bileþenini al
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Rastgele bir yön vektörü oluþtur
            Vector3 randomDirection = Random.onUnitSphere; // Birim küre üzerinde rastgele bir yön
            randomDirection.y = Mathf.Abs(randomDirection.y); // Yukarý yönü tercih etmek için Y pozitif olabilir

            // Kuvveti uygula
            rb.AddForce(randomDirection * forceAmount, ForceMode.Impulse);

            // Rastgele bir rotasyon uygula
            Vector3 randomTorque = Random.onUnitSphere * rotationSpeed; // Rastgele dönme momenti (torque)
            rb.AddTorque(randomTorque, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody bileþeni bulunamadý!");
        }

        // 2 saniye sonra hareketi ve rotasyonu durdur
        Invoke("StopMotionAndRotation", 2f);
    }

    void StopMotionAndRotation()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // Hareketi durdur
            rb.angularVelocity = Vector3.zero; // Rotasyonu durdur
        }

        isStopped = true;
    }

    void Update()
    {
        if (isStopped && !die)
        {
            // Durdurulduktan sonra pozisyon ve rotasyonu kilitle
            rb.constraints = RigidbodyConstraints.FreezeAll;
            die = true;
        }
    }
}
