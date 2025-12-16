using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 
using UnityEngine.Networking;
using UnityEngine.UI;
using soal;
public class JawabanManager : MonoBehaviour
{
    public GamesRules rules;
    public ScoreManager skor;
    private string submitUrl = "https://irul.aplikassi.my.id/submitJawaban.php"; // Ganti dengan URL PHP Anda
    private string skorUrl = "https://irul.aplikassi.my.id/submitSkor.php"; // Ganti dengan URL PHP Anda
    private List<Jawaban> jawabanList = new List<Jawaban>();
    public SoalManager1 soal;
    public string studentId;
    public int jumlahSoal;
    // Start is called before the first frame update
    void Start()
    {
        studentId = PlayerPrefs.GetString("StudentId");
    }
    public void setJumlahSoal(int j){
        jumlahSoal = j;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    public IEnumerator SendSkor(int sc,string l)
    {
        WWWForm form = new WWWForm();
        form.AddField("skor", sc.ToString());
        form.AddField("level", l);
        form.AddField("id_siswa", studentId);
        using (UnityWebRequest webRequest = UnityWebRequest.Post(skorUrl, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Jawaban berhasil dikirim: " + webRequest.downloadHandler.text);
                Debug.Log(form);
                // Tambahkan logika untuk menangani respon server jika diperlukan
            }
            else
            {
                Debug.LogError("Terjadi kesalahan saat mengirim jawaban: " + webRequest.error);
            }
        }

    }
    public IEnumerator SendAnswers()
    {
        WWWForm form = new WWWForm();
         for (int i = 0; i < jawabanList.Count; i++)
            {
                var jawaban = jawabanList[i];
                form.AddField($"jawaban[{i}][id_soal]", jawaban.id_soal);
                form.AddField($"jawaban[{i}][jawaban]", jawaban.jawaban);
                form.AddField($"jawaban[{i}][nilai]", jawaban.nilai.ToString());
                form.AddField($"jawaban[{i}][id_siswa]", jawaban.id_siswa);
            
                
            }

        using (UnityWebRequest webRequest = UnityWebRequest.Post(submitUrl, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Jawaban berhasil dikirim: " + webRequest.downloadHandler.text);
                Debug.Log(form);
                // Tambahkan logika untuk menangani respon server jika diperlukan
            }
            else
            {
                Debug.LogError("Terjadi kesalahan saat mengirim jawaban: " + webRequest.error);
            }
        }
    }
    
    public void JawabSoal(string pilihan,int score,int index)
    {
        
        if (soal.soals != null && index < soal.soals.Count)
        {
            int currentScore = 0;
            Soal currentSoal = soal.soals[index];
            if (pilihan == currentSoal.jawaban) // Jika jawaban benar
            {                           

                currentScore = rules.JawabanBenar(currentSoal.Level);
                score += currentScore;
            }
            else // Jika jawaban salah
            {
                rules.minusLive(studentId ,score);
                currentScore = rules.JawabanSalah(currentSoal.Level);
                if (score > 0)
                {
                    score = score + currentScore;
                    if (score < 0)
                    {
                        score = 0; // Pastikan skor tidak kurang dari 0
                    }
                    
                }
                
            }
            soal.setSkor(score);
            jawabanList.Add(new Jawaban(currentSoal.id_soal, pilihan, currentScore, studentId));
            
            skor.UpdateScoreText(score);
            rules.NextSoal(index,jumlahSoal,studentId);
             
        }
    }
    public void JawabSoal(string pilihan,int cs,int score,int index)
    {
        if (soal.soals != null && index < soal.soals.Count)
        {
            Soal currentSoal = soal.soals[index];
            
            jawabanList.Add(new Jawaban(currentSoal.id_soal, pilihan, cs, studentId));
            
            skor.UpdateScoreText(score);
            rules.NextSoal(index,jumlahSoal,studentId); 
        }
    }
    [System.Serializable]
    public class Jawaban
    {
        public string id_soal;
        public string jawaban;
        public int nilai;
        public string id_siswa;

        public Jawaban(string idSoal, string jawaban, int nilai, string idSiswa)
        {
            this.id_soal = idSoal;
            this.jawaban = jawaban;
            this.nilai = nilai;
            this.id_siswa = idSiswa;
        }
    }
    
 
}

