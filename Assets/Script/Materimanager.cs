using UnityEngine;
using UnityEngine.SceneManagement;

public class Materimanager : MonoBehaviour
{
    // Fungsi untuk setiap tombol materi
    public void SelectMateri(int materiIndex)
    {
        // Daftar path video relatif
        string[] videoPaths = {
            "materi1.mp4",
            "materi2.mp4",
            "materi3.mp4",
            "materi4.mp4",
            "materi5.mp4",
            "materi6.mp4",
            "materi7.mp4",
            "materi8.mp4",
            "materi9.mp4"

        };

        if (materiIndex >= 0 && materiIndex < videoPaths.Length)
        {
            // Menyimpan path video ke PlayerPrefs
            PlayerPrefs.SetString("SelectedVideoPath", videoPaths[materiIndex]);
            PlayerPrefs.Save();

            // Pindah ke scene video
            SceneManager.LoadScene("Video"); // Ganti dengan nama scene Anda
        }
        else
        {
            Debug.LogWarning("Indeks materi tidak valid: " + materiIndex);
        }
    }
}
