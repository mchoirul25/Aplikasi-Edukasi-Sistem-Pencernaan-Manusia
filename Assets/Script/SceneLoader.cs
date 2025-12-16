using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public string soalSceneName = "soal"; // Nama scene soal
    public string previousSceneName = "Level"; // Nama scene sebelumnya (misalnya Menu)
    public Button menengah,sulit;
    string studentId;
    int lives;
     
    public GameObject canvas; 
    void Start()
    {
        studentId = PlayerPrefs.GetString("StudentId");
        // Cek nyawa saat mencoba masuk ke scene soal
        lives = PlayerPrefs.GetInt(studentId+"_lives");
        
        Debug.Log(lives);
        canvas.SetActive(false);
        // PlayerPrefs.SetString("accomplishe","no");
        // PlayerPrefs.Save();
        CekLevel();
        
    }
    void CekLevel(){
        menengah.interactable = false;
        sulit.interactable = false;
        string level = PlayerPrefs.GetString(studentId+"_accomplishe");
        
        if(level == "mediumDone"){
            menengah.interactable = true;
            sulit.interactable = true;
        }else if(level == "easyDone"){
            menengah.interactable = true;
        }
    }
    
    public void LoadMudah(){
        PlayerPrefs.SetString("level","easy");
        LoadSoalScene();
    }
    public void LoadMenengah(){
        PlayerPrefs.SetString("level","medium");
        LoadSoalScene();
    }
    public void LoadSulit(){
        PlayerPrefs.SetString("level","hard");
        LoadSoalScene();
    }
    public void LoadSoalScene()
    {
        if (lives <= 0)
        {
            // Jika nyawa 0, kembalikan ke scene sebelumnya
            canvas.SetActive(true);
        }
        else
        {
            // Masuk ke scene soal
            SceneManager.LoadScene(soalSceneName);
        }
    }
    public void kembali(){
        SceneManager.LoadScene("Main_Menu");
    }
    
}
