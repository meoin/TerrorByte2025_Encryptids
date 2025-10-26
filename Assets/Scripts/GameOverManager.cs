using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    private float currentOpacity = 0f;
    private float samOpacity = 0f;
    private float timeDelayBeforeMenu = 5f;
    private float menuLoadTimer = 0f;
    public Image gameOverImage;
    public Image sam;

    // Update is called once per frame
    void Update()
    {
        currentOpacity = Mathf.Min(currentOpacity + 0.001f, 1f);

        Color alphaColor = gameOverImage.color;
        alphaColor.a = currentOpacity;
        gameOverImage.color = alphaColor;

        Color samColor = sam.color;
        samColor.a = samOpacity;
        sam.color = samColor;

        if (currentOpacity >= 1f)
        {
            menuLoadTimer += Time.deltaTime;
            samOpacity = Mathf.Min(samOpacity + 0.00001f, 1f);
        }

        if (menuLoadTimer >= timeDelayBeforeMenu) 
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
