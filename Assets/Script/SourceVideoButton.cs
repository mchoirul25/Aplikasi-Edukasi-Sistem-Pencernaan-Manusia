using UnityEngine;
using UnityEngine.UI;

public class SourceVideoButton : MonoBehaviour
{
    public GameObject descriptionPanel; // Panel yang akan menampilkan deskripsi
    public Text descriptionText; // Text yang menampilkan deskripsi sumber video

    void Start()
    {
        // Pastikan panel deskripsi tidak terlihat saat awal permainan
        if (descriptionPanel != null)
        {
            descriptionPanel.SetActive(false);
        }
    }

    // Fungsi untuk menampilkan deskripsi sumber video
    public void ShowDescription()
    {
        if (descriptionPanel != null)
        {
            descriptionPanel.SetActive(true); // Menampilkan panel
            
        }
    }

    // Fungsi untuk menyembunyikan deskripsi sumber video
    public void HideDescription()
    {
        if (descriptionPanel != null)
        {
            descriptionPanel.SetActive(false); // Menyembunyikan panel
        }
    }
}
