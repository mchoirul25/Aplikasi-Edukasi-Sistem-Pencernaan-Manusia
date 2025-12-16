using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamesRules : MonoBehaviour
{
    public Text liveText;
    public SoalManager1 soal;
    public TimerManager timer;
    public ScoreManager skor;
    private string level;
    public int correctStreak = 0;
    private int lives; // Tambahkan definisi nyawa
    private int jawabanBenar;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        
        level = PlayerPrefs.GetString("level");;
        Debug.Log(level);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setLives(string studentId){
        lives = PlayerPrefs.GetInt(studentId + "_lives");
        // lives = 50;
        liveText.text = lives.ToString();
    }
     public void NextSoal(int currentIndex, int jumlahSoal,string id)
    {
        // Periksa jika index saat ini tidak di luar batas maksimum list soal
        Debug.Log(currentIndex);
        Debug.Log(jumlahSoal);
        if (currentIndex < jumlahSoal)
        {
            soal.currentIndex++;
            soal.DisplayCurrentSoal();
        }
        else
        {
            // Menampilkan evaluasi di akhir sesi
            soal.SubmitAnswer();
            
            stopTimer();
            nextGame(id);
            // Tambahkan evaluasi atau hasil akhir di sini
        }
    }
    void stopTimer(){
        timer.timerText.text = "selesai";
        soal.pilihanAButton.interactable = false;
        soal.pilihanBButton.interactable = false;
        soal.pilihanCButton.interactable = false;
        soal.pilihanDButton.interactable = false;

    }
    public void minusLive(string id, int sc){
        lives--;
        liveText.text = lives.ToString();
        PlayerPrefs.SetInt(id + "_lives", lives);// Simpan nyawa terbaru ke PlayerPrefs
        PlayerPrefs.Save(); // Pastikan data tersimpan
        Debug.Log("Jawaban salah. Nyawa tersisa: " + lives);
        correctStreak = 0;
        // Jika nyawa habis, akhiri permainan
        if (lives < 1)
        {
            Debug.Log("Jawaban salah. Nyawa tersisa: " + lives);
            GameOver(sc);
        }
    }
    void GameOver(int sc)
    {
        // Logika untuk mengakhiri permainan jika nyawa habis
        PlayerPrefs.SetInt("PlayerScore", sc);
        PlayerPrefs.SetInt("NextGame", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene("PopUp");
        // Contoh: Bisa menampilkan pesan Game Over atau reset permainan
    }
    public int JawabanBenar(string soalLevel){
        int basePoints = skor.GetPointsForDifficulty(soalLevel); // Ambil poin berdasarkan tingkat kesulitan
        int streak = AddStreakBonus(correctStreak); // Cek apakah ada bonus untuk streak
        int currentScore = basePoints + streak;
        correctStreak++;
        jawabanBenar++;
        return currentScore;
         // Tambah streak jawaban benar
    }
    public int JawabanSalah(string soalLevel){
        
         // Reset streak jawaban benar
        correctStreak = 0;
        int penalti = ApplyPenalty(soalLevel); // Terapkan penalti berdasarkan kesulitan
        return penalti;
    }

    public void nextGame(string id)
    {
        
        float akumulasi = (float)jawabanBenar / soal.soals.Count; // Pastikan pembagian menghasilkan float    
        Debug.Log("akumulasi benar"+akumulasi);
        Debug.Log("jawaban benar"+jawabanBenar);
        Debug.Log("jumsol"+soal.soals.Count);
        if (akumulasi >= 0.7f)
        { 
            PlayerPrefs.SetInt("NextGame", 1);
            if (level == "easy")
            {
                PlayerPrefs.SetString(id + "_accomplishe", "easyDone");
            }
            else if (level == "medium")
            {
                PlayerPrefs.SetString(id + "_accomplishe", "mediumDone");
            }
            PlayerPrefs.Save();
            Debug.Log("Level selesai, data disimpan.");
        }
        else
        {
            PlayerPrefs.SetInt("NextGame", 0);
            Debug.Log("Tidak memenuhi syarat untuk next level.");
        }

        PlayerPrefs.SetInt("PlayerScore", soal.score);
        PlayerPrefs.Save();
        SceneManager.LoadScene("PopUp");
    }
   // Function to apply penalty based on difficulty
    public int ApplyPenalty(string difficulty)
    {
        switch (difficulty)
        {
            case "easy":
                return -5;
            case "medium":
                return -10;
            case "hard":
                return -15;
        }
        return 0;
    }

    // Function to add streak bonus if applicable
    public int AddStreakBonus(int correctStreak)
    {
        int streak = 0;
        if (correctStreak == 3)
        {
            streak = 20; // Bonus untuk 3 jawaban benar berturut-turut
            Debug.Log("Bonus streak 3 jawaban benar!");
        }
        else if (correctStreak == 5)
        {
            streak = 50; // Bonus untuk 5 jawaban benar berturut-turut
            Debug.Log("Bonus streak 5 jawaban benar!");
        }
        return streak;
    }
}
