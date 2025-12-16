using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Networking;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign di Inspector
    public RawImage rawImage; // Assign di Inspector
    public Button playButton, pauseButton, backButton; // Assign di Inspector
    public Text textNyawa, text1, text2;
    public GameObject notificationPanel;
    public float notificationDuration = 3f;

    private string studentId;
    private int sisaNyawa;
    private bool videoFinished = false;

    void Start()
    {
        studentId = PlayerPrefs.GetString("StudentId");
        sisaNyawa = PlayerPrefs.GetInt(studentId + "_lives", 0); // Default lives = 0

        string selectedVideoPath = PlayerPrefs.GetString("SelectedVideoPath", "");

        notificationPanel.SetActive(false);

        // Muat video berdasarkan path
        if (!string.IsNullOrEmpty(selectedVideoPath))
        {
            LoadVideoByPath(selectedVideoPath);
        }

        // Tambahkan event listener untuk tombol Play dan Pause
        playButton.onClick.AddListener(PlayVideo);
        pauseButton.onClick.AddListener(PauseVideo);

        // Tambahkan listener untuk event video selesai
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    public void LoadVideoByPath(string relativePath)
    {
        string streamingAssetsPath = Path.Combine(Application.streamingAssetsPath, relativePath);

        if (Application.platform == RuntimePlatform.Android)
        {
            StartCoroutine(LoadVideoAndroid(streamingAssetsPath));
        }
        else
        {
            SetVideoPath(streamingAssetsPath);
        }
    }

    private IEnumerator LoadVideoAndroid(string path)
    {
        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string persistentPath = Path.Combine(Application.persistentDataPath, Path.GetFileName(path));
            File.WriteAllBytes(persistentPath, request.downloadHandler.data);
            SetVideoPath(persistentPath);
        }
        else
        {
            Debug.LogError("Gagal memuat video: " + request.error);
        }
    }

    private void SetVideoPath(string path)
    {
        if (File.Exists(path))
        {
            videoPlayer.url = path;
            videoPlayer.Prepare();

            RenderTexture renderTexture = new RenderTexture(640, 360, 0);
            videoPlayer.targetTexture = renderTexture;
            rawImage.texture = renderTexture;
        }
        else
        {
            Debug.LogWarning("Video tidak ditemukan: " + path);
        }
    }

    public void PlayVideo()
    {
        if (videoPlayer != null && !videoPlayer.isPlaying)
        {
            videoPlayer.Play();
            Debug.Log("Video diputar.");
        }
    }

    public void PauseVideo()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            Debug.Log("Video dijeda.");
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        videoFinished = true;
        sisaNyawa = logicNyawa(sisaNyawa);
        PlayerPrefs.SetInt(studentId + "_lives", sisaNyawa);
        PlayerPrefs.Save();

        text1.text = "Selamat Kamu Telah Menonton Video Materi Sampai Selesai!";
        text2.text = "Heart Kamu Bertambah 1";
        textNyawa.text = sisaNyawa.ToString();

        StartCoroutine(ShowNotificationAndChangeScene());
    }

    private int logicNyawa(int nyawa)
    {
        return nyawa < 5 ? nyawa + 1 : 5;
    }

    private IEnumerator ShowNotificationAndChangeScene()
    {
        notificationPanel.SetActive(true);
        yield return new WaitForSeconds(notificationDuration);
        notificationPanel.SetActive(false);
        SceneManager.LoadScene("Menu Video");
    }

    public void OnBackButtonClick()
    {
        if (!videoFinished)
        {
            text1.text = "Kamu Belum Menonton Video Materi Sampai Selesai";
            text2.text = "Heart Kamu Tidak Bertambah";
            textNyawa.text = sisaNyawa.ToString();

            StartCoroutine(ShowNotificationAndChangeScene());
        }
    }
}
