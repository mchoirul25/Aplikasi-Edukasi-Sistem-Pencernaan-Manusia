using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public Text scoreText; 
    public GameObject bg_c; // Background Congrats
    public GameObject bg_g; // Background Game Over
    public GameObject popup; // Pop-up Notifikasi

    void Start()
    {
        // Ambil skor dan status game
        int score = PlayerPrefs.GetInt("PlayerScore", 0); 
        int cek = PlayerPrefs.GetInt("NextGame", 0);
        Debug.Log("Cek: " + cek);

        // Tampilkan skor di UI
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
        else
        {
            Debug.LogError("Score Text UI is not assigned in the Inspector.");
        }

        // Tentukan background & atur pop-up
        if (cek == 1) // Jika menang
        {
            bg_g.SetActive(false);
            bg_c.SetActive(true);
            if (popup != null) popup.SetActive(false); // Pastikan pop-up tidak muncul
        }
        else // Jika game over
        {
            bg_c.SetActive(false);
            bg_g.SetActive(true);
            ShowPopup(); // Tampilkan pop-up hanya saat game over
        }
    }

    void ShowPopup()
    {
        if (popup != null)
        {
            popup.SetActive(true);
            Invoke("HidePopup", 5f); // Sembunyikan pop-up setelah 3 detik
        }
    }

    void HidePopup()
    {
        if (popup != null) popup.SetActive(false);
    }

    public void Main_Menu()
    {
        int cek = PlayerPrefs.GetInt("NextGame", 0);
        if (cek == 1)
        {
            SceneManager.LoadScene("Level"); // Jika menang, kembali ke Level
        }
        else
        {
            SceneManager.LoadScene("Menu Video"); // Jika game over, masuk ke materi
        }
        Debug.Log("Kembali ke " + (cek == 1 ? "Level" : "Materi"));
    }
}
