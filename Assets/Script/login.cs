using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public GameObject canvas;
    public Text Pesan;
    // public Text messageText;
    public Button loginButton; 

    private string loginUrl = "https://irul.aplikassi.my.id/login.php"; // Sesuaikan dengan URL PHP Anda

    void Start()
    {
        canvas.SetActive(false);
        loginButton.onClick.AddListener(Login); // Tambahkan listener untuk tombol login
    }

    public void Login()
    {
        StartCoroutine(LoginRequest());
    }

    IEnumerator LoginRequest()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;
        string jsonData = JsonUtility.ToJson(new LoginData(username, password));

        // Log JSON data yang dikirim
        Debug.Log("Data yang dikirim: " + jsonData);

        UnityWebRequest www = new UnityWebRequest(loginUrl, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            // messageText.text = "Error: " + www.error;
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(www.downloadHandler.text);
            // messageText.text = response.message;

            if (response.status == "success")
            {

                PlayerPrefs.SetString("StudentId", username);
                PlayerPrefs.Save();
                
                SceneManager.LoadScene("Main_Menu");
            }else{
                canvas.SetActive(true);
                Pesan.text = "Maaf Anda Tidak Bisa Masuk " + response.message;
            }
        }
    }

    [System.Serializable]
    public class LoginResponse
    {
        public string status;
        public string message;
    }
    [System.Serializable]
    public class LoginData
    {
        public string username;
        public string password;

        public LoginData(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
