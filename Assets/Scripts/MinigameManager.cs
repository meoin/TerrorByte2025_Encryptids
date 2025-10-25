using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    private bool gameBeginning = false;
    private float fadeInOpacity = 0f;
    public float fadeInSpeed = 0.001f;
    public bool gameStarted = false;
    public bool canShoot = false;
    private bool opponentShot = false;
    private bool playerShot = false;
    private float shootTimer = 0f;
    private float shootTimeLimit;
    public float opponentShootTime;
    public int winsRequired = 3;
    private int playerWins = 0;
    private int opponentWins = 0;
    public AudioSource shootPromptSFX;
    public Image shootPromptUI;
    public TextMeshProUGUI playerWinText;
    public TextMeshProUGUI opponentWinText;
    public GameObject crosshair;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootTimeLimit = Random.Range(5f, 10f);
        UpdateWinUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            shootTimer += Time.deltaTime;

            if (shootTimer > shootTimeLimit && !canShoot)
            {
                TimeLimitReached();
            }
            else if (shootTimer > shootTimeLimit && canShoot)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (crosshair.GetComponent<CrosshairMovement>().CrosshairIsOverOpponent())
                    {
                        opponentShot = true;
                        playerWins++;
                        UpdateWinUI();
                        shootPromptSFX.Play();
                        ResetMinigame();
                    }
                }
            }

            float realOpponentTimer = opponentShootTime * Mathf.Min(1 + (opponentWins * 0.15f), 1.5f);

            if (shootTimer > shootTimeLimit + realOpponentTimer && !opponentShot)
            {
                canShoot = false;
                playerShot = true;
                opponentWins++;
                UpdateWinUI();
                shootPromptSFX.Play();
                ResetMinigame();
            }
        }
        else if (gameBeginning) 
        {
            MinigameFadeIn();
        }
    }

    public void StartMinigame() 
    {
        gameBeginning = true;
        crosshair.SetActive(true);
        playerWinText.gameObject.SetActive(true);
        opponentWinText.gameObject.SetActive(true);
    }

    void MinigameFadeIn() 
    {
        fadeInOpacity += fadeInSpeed;

        Color crosshairColor = crosshair.gameObject.GetComponent<Image>().color;
        crosshairColor.a = fadeInOpacity;
        crosshair.gameObject.GetComponent<Image>().color = crosshairColor;

        Color textColor = playerWinText.color;
        textColor.a = fadeInOpacity;
        playerWinText.color = textColor;
        opponentWinText.color = textColor;

        if (fadeInOpacity >= 1f) 
        {
            gameBeginning = false;
            gameStarted = true;
        }

    }

    void UpdateWinUI() 
    {
        playerWinText.text = $"You: {playerWins}";
        opponentWinText.text = $"Opp: {opponentWins}";
    }

    void TimeLimitReached() 
    {
        shootPromptSFX.Play();
        shootPromptUI.gameObject.SetActive(true);
        canShoot = true;
    }

    void ResetMinigame() 
    {
        shootTimer = 0f;
        shootTimeLimit = Random.Range(5f, 10f);
        canShoot = false;
        opponentShot = false;
        playerShot = false;
        shootPromptUI.gameObject.SetActive(false);
    }
}
