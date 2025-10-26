using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName;
    public Image fadePanel;
    private bool videoFinished = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (videoFinished) 
        {
            Color panelColor = fadePanel.color;
            panelColor.a = panelColor.a + 0.01f;
            fadePanel.color = panelColor;

            if (panelColor.a >= 1) 
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    void OnVideoFinished(VideoPlayer vp) 
    {
        videoFinished = true;
    }
}
