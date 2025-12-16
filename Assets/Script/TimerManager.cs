using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public Text timerText;
    private float easyTimer = 30f, mediumTimer = 20f, hardTimer = 15f; 
     // Waktu yang tersisa untuk soal saat ini
    private float totalTimePerQuestion;
    // private bool isTimerRunning = false; // Status timer apakah timer berjalan
    public bool isTimerRunning
    {
        get { return isTimerRunning; } // Getter
        set { isTimerRunning = value; } // Setter
    }
    
    public void updateTimerText(){

    }
    public float GetTimerForDifficulty(string difficulty)
    {
        switch (difficulty) // Misal 'difficulty' adalah properti di dalam kelas Soal
        {
            case "easy":
                return easyTimer;
                break;
            case "medium":
                return mediumTimer;
                break;
            case "hard":
                return hardTimer;
                break;
            default:
                return mediumTimer; // Nilai default jika tidak ada level yang cocok
                break;
        }
        return totalTimePerQuestion;
    }

}
