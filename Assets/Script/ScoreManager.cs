using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    private int easyPoints = 10;
    private int mediumPoints = 20;
    private int hardPoints = 30;
    
    
    public int GetPointsForDifficulty(string difficulty)
    {
        switch (difficulty)
        {
            case "easy":
                return 10;
            case "medium":
                return 20;
            case "hard":
                return 30;
            default:
                return 0;
        }
    }
    public void UpdateScoreText(int score)
    {
        // Update tampilan skor
        scoreText.text = score.ToString();
    }
}
