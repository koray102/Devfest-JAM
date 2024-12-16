using UnityEngine;

public class RandomForce : MonoBehaviour
{
    public float forceAmount = 10f; // Kuvvet miktar�
    public float rotationSpeed = 100f; // Rotasyon h�z�

    private Rigidbody rb;
    private bool isStopped = false; // Hareketi ve rotasyonu durdurma kontrol�
    private bool die = false;
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

            // Rastgele bir rotasyon uygula
            Vector3 randomTorque = Random.onUnitSphere * rotationSpeed; // Rastgele d�nme momenti (torque)
            rb.AddTorque(randomTorque, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody bile�eni bulunamad�!");
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
