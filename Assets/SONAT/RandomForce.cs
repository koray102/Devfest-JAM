using UnityEngine;

public class RandomForce : MonoBehaviour
{
    public float forceAmount = 10f; // Uygulamak istedi�iniz kuvvet miktar�

    private Rigidbody rb;

    void Start()
    {
        // Rigidbody bile�enini al
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Rastgele bir y�n vekt�r� olu�tur
            Vector3 randomDirection = Random.onUnitSphere; // Birim k�re �zerinde rastgele bir y�n
            randomDirection.y = Mathf.Abs(randomDirection.y); // Yukar� y�n� tercih etmek i�in Y pozitif olabilir

            // Kuvveti uygula
            rb.AddForce(randomDirection * forceAmount, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody bile�eni bulunamad�!");
        }
    }
}
