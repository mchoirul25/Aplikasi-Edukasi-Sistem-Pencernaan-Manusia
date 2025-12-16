using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private string userID;

    public void Main_Menu()
    {
        SceneManager.LoadScene("Main_Menu");
        Debug.Log("kembali");
    }

    public void tentang()
    {
        SceneManager.LoadScene("Tentang");
    }

    public void materi()
    {
        SceneManager.LoadScene("Menu Video");
    }

    public void petunjuk()
    {
        SceneManager.LoadScene("Petunjuk");
    }

    public void soal()
    {
        SceneManager.LoadScene("Level");
    }

    public void nilai()
    {
        SceneManager.LoadScene("Nilai");
    }

    public void Logout()
    {
        // Ambil data nyawa saat ini untuk memastikan tetap tersimpan
        string studentId = PlayerPrefs.GetString("StudentId");

        if (!string.IsNullOrEmpty(studentId))
        {
            int currentLives = PlayerPrefs.GetInt(studentId + "_lives", 0); // Ambil jumlah nyawa saat ini
            int currentLevel = PlayerPrefs.GetInt(studentId + "_lastLevel", 0); // Ambil level terakhir (opsional)

            Debug.Log($"Logout berhasil, data disimpan: Nyawa = {currentLives}, Level = {currentLevel}");
        }

        // Hapus hanya StudentId agar sesi login baru diperlukan
        PlayerPrefs.DeleteKey("StudentId");

        // Simpan perubahan
        PlayerPrefs.Save();

        // Pindah ke scene login
        SceneManager.LoadScene("Login");
    }


}
