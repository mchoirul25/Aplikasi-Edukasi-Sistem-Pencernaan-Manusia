using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using soal;

public class SoalManager1 : MonoBehaviour
{
    public Text soalText; // Legacy Text untuk pertanyaan
    public Text pilihanAText; // Legacy Text untuk pilihan A
    public Text pilihanBText; // Legacy Text untuk pilihan B
    public Text pilihanCText; // Legacy Text untuk pilihan C
    public Text pilihanDText; // Legacy Text untuk pilihan D
     // Text untuk menampilkan live

    public Button pilihanAButton; // Tombol untuk pilihan A
    public Button pilihanBButton; // Tombol untuk pilihan B
    public Button pilihanCButton; // Tombol untuk pilihan C
    public Button pilihanDButton; 
    public List<Soal> soals; // List untuk menampung data soal
    private List<string[]> jawabanPengguna = new List<string[]>(); // Menyimpan jawaban pengguna
    public float currentTime;
    public int currentIndex = 0; // Index soal yang sedang ditampilkan
     // Skor pemain
    public TimerManager timer;
    public JawabanManager jawab;
    public ScoreManager skor;
    public GamesRules rules;
    public int score = 0;
    HashSet<string> usedQuestions = new HashSet<string>();
    public int jawabanBenar;

    private string studentId;
    public string level;
    // public int lives;
    public int jumlahSoal;
    void Start()
    {
        StartCoroutine(GetSoalData());
        studentId = PlayerPrefs.GetString("StudentId");
        rules.setLives(studentId);
        
        
        if (pilihanAButton != null)
        {
            pilihanAButton.onClick.AddListener(() => jawab.JawabSoal("A",score,currentIndex));
        }
        if (pilihanBButton != null)
        {
            pilihanBButton.onClick.AddListener(() => jawab.JawabSoal("B",score,currentIndex));
        }
        if (pilihanCButton != null)
        {
            pilihanCButton.onClick.AddListener(() => jawab.JawabSoal("C",score,currentIndex));
        }
        if (pilihanDButton != null)
        {
            pilihanDButton.onClick.AddListener(() => jawab.JawabSoal("D",score,currentIndex));
        }
        
    }

    IEnumerator GetSoalData()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://irul.aplikassi.my.id/getSoal.php"); // Ganti dengan URL API Anda
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            string jsonResult = www.downloadHandler.text;
            SoalResponse response = JsonUtility.FromJson<SoalResponse>(jsonResult);

            if (response.status == "success")
            {
           

            soals = pilihLevel(response);
            jumlahSoal = soals.Count - 1;
            jawab.setJumlahSoal(jumlahSoal);
            DisplayCurrentSoal();
            skor.UpdateScoreText(0); // Menampilkan skor awal
            }
            else
            {
                Debug.LogError("Failed to fetch soal data: " + jsonResult);
            }
        }
    }
    public void setSkor(int s){
        score = s;
    }
    
    List<Soal> pilihLevel(SoalResponse response){
        int totalQuestions = 10; // Total number of questions to include
        int easyCount = 1, mediumCount = 1, hardCount = 1;
        level = PlayerPrefs.GetString("level");
        // level = "hard";
        List<Soal> easySoals = new List<Soal>();
        List<Soal> mediumSoals = new List<Soal>();
        List<Soal> hardSoals = new List<Soal>();

        foreach (var soal in response.data)
        {
            switch (soal.Level)
            {
                case "easy":
                    easySoals.Add(soal);
                    break;
                case "medium":
                    mediumSoals.Add(soal);
                    break;
                case "hard":
                    hardSoals.Add(soal);
                    break;
            }
        }

        // Ambil soal berdasarkan proporsi yang diinginkan
        List<Soal> selectedSoals = new List<Soal>();
        if(level == "easy"){
            easyCount = (int)(totalQuestions * 0.60f);
            mediumCount = (int)(totalQuestions * 0.30f);
            hardCount = (int)(totalQuestions * 0.10f);
        }else if(level == "medium"){
            easyCount = (int)(totalQuestions * 0.30f);
            mediumCount = (int)(totalQuestions * 0.50f);
            hardCount = (int)(totalQuestions * 0.20f);
        }else if(level == "hard"){
            easyCount = (int)(totalQuestions * 0.10f);
            mediumCount = (int)(totalQuestions * 0.20f);
            hardCount = totalQuestions - easyCount - mediumCount;
        }
         // Mengacak soal menggunakan Fisher-Yates
        easySoals = FisherYatesShuffle(easySoals);
        mediumSoals = FisherYatesShuffle(mediumSoals);
        hardSoals = FisherYatesShuffle(hardSoals);

        // Menambahkan soal ke daftar terpilih dengan memeriksa apakah soal sudah pernah dipilih
       
        selectedSoals.AddRange(GetUniqueSoals(easySoals, easyCount));  
        selectedSoals.AddRange(GetUniqueSoals(mediumSoals, mediumCount)); 
        selectedSoals.AddRange(GetUniqueSoals(hardSoals, hardCount));  

        // Campurkan soal secara acak
        selectedSoals = FisherYatesShuffle(selectedSoals);

        return selectedSoals;
    }
    List<Soal> GetUniqueSoals(List<Soal> soals, int count)
    {
        List<Soal> uniqueSoals = new List<Soal>();
        foreach (var soal in soals)
        {
            if (uniqueSoals.Count >= count)
                break;

            if (!usedQuestions.Contains(soal.id_soal))
            {
                usedQuestions.Add(soal.id_soal);
                uniqueSoals.Add(soal);
            }
        }

        return uniqueSoals;
    }
    void Update()
    {
        if (currentTime > 0)
        {
            Soal currentSoal = soals[currentIndex];
            currentTime -= Time.deltaTime;
            timer.timerText.text = Mathf.Ceil(currentTime).ToString();

            if (currentTime <= 0)
            {
                jawab.JawabSoal("waktu habis",score,currentIndex);               
            
            }
        }
    }

    public void DisplayCurrentSoal()
    {
        // Menampilkan soal berdasarkan index yang saat ini aktif
        if (soals != null && soals.Count > 0 && currentIndex >= 0 && currentIndex < soals.Count)
        {
            Soal currentSoal = soals[currentIndex];
            soalText.text = currentSoal.pertanyaan;
            pilihanAText.text = currentSoal.A;
            pilihanBText.text = currentSoal.B;
            pilihanCText.text = currentSoal.C;
            pilihanDText.text = currentSoal.D;

            // Reset dan atur ulang timer untuk soal baru
            currentTime = timer.GetTimerForDifficulty(currentSoal.Level); 
            // timer.isTimerRunning = true;
        }
    }
    public void SubmitAnswer()
    {
        StartCoroutine(jawab.SendAnswers());
        StartCoroutine(jawab.SendSkor(score,level));
    }  
    
   
    public static List<T> FisherYatesShuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random(); // RNG untuk angka acak
        int n = list.Count; // Jumlah elemen dalam daftar
        while (n > 1) // Selama elemen yang belum diacak lebih dari 1
        {
            n--; // Kurangi n (mengecilkan rentang yang diacak)
            int k = rng.Next(n + 1); // Pilih indeks acak k (0 <= k <= n)
            T value = list[k]; // Ambil nilai pada indeks k
            list[k] = list[n]; // Tukar nilai list[k] dengan list[n]
            list[n] = value; // Simpan nilai list[k] pada indeks n
        }
        return list; // Kembalikan daftar teracak
    }    
    
    [System.Serializable]
    public class SoalResponse
    {
        public string status;
        public List<Soal> data;
    }
    

}
namespace soal
{
    [System.Serializable]
    public class Soal
    {
        public string id_soal;
        public string pertanyaan;
        public string jawaban;
        public string A;
        public string B;
        public string C;
        public string D;
        public string Level;
    }
}

