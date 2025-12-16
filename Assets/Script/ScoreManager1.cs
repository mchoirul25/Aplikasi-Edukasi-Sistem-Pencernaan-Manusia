using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager1 : MonoBehaviour
{
    private string baseUrl = "https://irul.aplikassi.my.id/getScores.php"; // Ganti dengan URL server PHP Anda

    public GameObject rowPrefab; // Prefab untuk setiap baris
    public Transform contentPanel; // Container untuk tabel (Panel dengan Vertical Layout Group)
    public int nomor = 1;
    void Start()
    {
        StartCoroutine(GetScores());
    }

    IEnumerator GetScores()
    {
        UnityWebRequest request = UnityWebRequest.Get(baseUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            
            ProcessScores(request.downloadHandler.text);
        }
    }
    void delete(){
        if (contentPanel.childCount > 0)
        {
            Transform firstChild = contentPanel.GetChild(0);
            Debug.Log("Destroying child: " + firstChild.gameObject.name);
            Destroy(firstChild.gameObject);
        }
    }
    void ProcessScores(string json)
    {
        ScoresData scoresData = JsonUtility.FromJson<ScoresData>(json);
        
        if (scoresData.status == "success")
        {
            foreach (var score in scoresData.data)
            {
                // Menampilkan siswa yang memiliki skor lebih dari 0 di salah satu level
                if (score.skor_mudah > 0 || score.skor_menengah > 0 || score.skor_sulit > 0)
                {
                    // Membuat instance dari rowPrefab
                    GameObject newRow = Instantiate(rowPrefab, contentPanel);

                    // Mengatur teks untuk kolom 'nama' dan 'total nilai'
                    Text noText = newRow.transform.Find("No/textNo").GetComponent<Text>();
                    Text namaText = newRow.transform.Find("Nama/textNama").GetComponent<Text>();
                    Text nilaiText = newRow.transform.Find("Nilai/textNilai").GetComponent<Text>();
                    int total = score.skor_mudah + score.skor_menengah + score.skor_sulit;
                    namaText.text = score.nama;
                    noText.text = nomor.ToString();
                    nilaiText.text = total.ToString();
                    nomor++;
                    
                }
            }
        }
        else
        {
            Debug.LogError("Error: " + scoresData.message);
        }
        delete();
    }

    [System.Serializable]
    public class ScoresData
    {
        public string status;
        public Score[] data;
        public string message;
    }

    [System.Serializable]
    public class Score
    {
        public string nama;
        public int skor_mudah;
        public int skor_menengah;
        public int skor_sulit;
    }
}
