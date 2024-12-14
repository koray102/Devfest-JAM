using UnityEngine;

public class RandomForce : MonoBehaviour
{
    public float forceAmount = 10f; // Uygulamak istediðiniz kuvvet miktarý

    private Rigidbody rb;

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
        }
        else
        {
            Debug.LogError("Rigidbody bileþeni bulunamadý!");
        }
    }
}
