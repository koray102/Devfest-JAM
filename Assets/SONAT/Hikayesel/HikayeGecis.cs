using UnityEngine;
using UnityEngine.UI;

public class HikayeGecis : MonoBehaviour
{
    public Sprite[] images; // Resimleri burada tanýmla
    public Image imageComponent; // Canvas'taki Image komponenti
    private int currentIndex = 0;
    public Image[] metinComponent;
    internal bool bitti = false;
   
    

    void Start()
    {
        if (images.Length > 0 && imageComponent != null)
        {
            imageComponent.sprite = images[currentIndex]; // Ýlk resmi yükle
        }
        else
        {
            Debug.LogError("Image array is empty or Image component is not assigned!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (currentIndex < images.Length - 1)
            {
                metinComponent[currentIndex].gameObject.SetActive(false);
                currentIndex++; // Bir sonraki resme geç
                imageComponent.sprite = images[currentIndex]; // Yeni resmi yükle
                metinComponent[currentIndex].gameObject.SetActive(true);

            }
            else
            {   
                bitti = true;
                gameObject.SetActive(false); // Canvas'ý kapat
            }
        }
    }
}

