using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    private bool gameBeginning = false;
    private float fadeInOpacity = 0f;
    public float fadeInSpeed = 0.001f;
    public bool gameStarted = false;
    public bool canShoot = false;
    public bool twinBattle = false;
    private bool firstTwinShot = false;
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
    public float resetTimeLimit = 1.5f;
    private float resetTimer = 0f;
    private bool roundReset = false;
    public bool theKid = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootTimeLimit = Random.Range(3f, 8f);
        UpdateWinUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            shootTimer += Time.deltaTime;

            if (shootTimer > shootTimeLimit && !canShoot && !roundReset)
            {
                TimeLimitReached();
            }
            else if (shootTimer > shootTimeLimit && canShoot)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (crosshair.GetComponent<CrosshairMovement>().CrosshairIsOverOpponent())
                    {
                        if (!twinBattle)
                        {
                            if (!theKid)
                            {
                                roundReset = true;
                                opponentShot = true;
                                playerWins++;
                                UpdateWinUI();
                                shootPromptUI.gameObject.SetActive(false);
                                shootPromptSFX.Play();
                                gameStarted = false;
                            }
                            else 
                            {
                                resetTimeLimit = 5f;
                                roundReset = true;
                                canShoot = false;
                                playerShot = true;
                                opponentWins++;
                                UpdateWinUI();
                                shootPromptUI.gameObject.SetActive(false);
                                shootPromptSFX.Play();
                                gameStarted = false;
                            }
                        }
                        else
                        {
                            if (!firstTwinShot)
                            {
                                firstTwinShot = true;
                            }
                            else
                            {
                                roundReset = true;
                                opponentShot = true;
                                playerWins++;
                                UpdateWinUI();
                                shootPromptUI.gameObject.SetActive(false);
                                shootPromptSFX.Play();
                                gameStarted = false;
                            }
                        }
                    }
                }
            }

            float realOpponentTimer = opponentShootTime * Mathf.Min(1 + (opponentWins * 0.15f), 1.5f);

            if (shootTimer > shootTimeLimit + realOpponentTimer && !opponentShot && !playerShot)
            {
                roundReset = true;
                canShoot = false;
                playerShot = true;
                opponentWins++;
                UpdateWinUI();
                shootPromptUI.gameObject.SetActive(false);
                shootPromptSFX.Play();
                gameStarted = false;
            }
        }
        else if (gameBeginning)
        {
            MinigameFadeIn();
        }

        if (roundReset)
        {
            //Debug.Log("Resetting round");
            resetTimer += Time.deltaTime;

            if (resetTimer >= resetTimeLimit)
            {
                resetTimer = 0f;
                ResetMinigame();
            }
        }
        Debug.Log($"Round timer: {shootTimer} / {shootTimeLimit}");
    }

    public int GetPlayerWins() 
    {
        return playerWins;
    }

    void RoundLost() 
    {
        roundReset = false;
        canShoot = false;
        playerShot = true;
        opponentWins++;
        UpdateWinUI();
        shootPromptSFX.Play();
        ResetMinigame();
    }

    void RoundWon() 
    {
        roundReset = false;
        opponentShot = true;
        playerWins++;
        UpdateWinUI();
        shootPromptSFX.Play();
        ResetMinigame();
    }

    public void StartMinigame() 
    {
        gameBeginning = true;
        crosshair.SetActive(true);
        if (!theKid) 
        {
            playerWinText.gameObject.SetActive(true);
            opponentWinText.gameObject.SetActive(true);
        }
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

    void ResetOpponents() 
    {
        GameObject[] opponents = GameObject.FindGameObjectsWithTag("Opponent");

        foreach (GameObject opponent in opponents)
        {
            if (opponent.TryGetComponent<SpriteLookAtCamera>(out SpriteLookAtCamera opp)) 
            {
                opp.StandBackUp();
            }

        }
    }

    void ResetMinigame() 
    {
        if (playerWins < winsRequired && opponentWins < winsRequired)
        {
            firstTwinShot = false;
            ResetOpponents();
            shootPromptUI.gameObject.SetActive(false);
            shootTimer = 0f;
            shootTimeLimit = Random.Range(3f, 8f);
            canShoot = false;
            opponentShot = false;
            playerShot = false;
            gameStarted = true;
            roundReset = false;
        }
        else if (playerWins >= winsRequired)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (opponentWins >= winsRequired) 
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
