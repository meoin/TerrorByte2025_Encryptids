using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private bool startGameInit = false;
    private bool gameStarted = false;
    private int startGameTimerLength = 3;
    private float startGameTimer = 0;

    public GameObject flashPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startGameInit && !gameStarted)
        { 
            
            //flashAnimator.Play("MenuLightFlash");
            gameStarted = true;
        }
        else if (gameStarted)
        {
            startGameTimer += Time.deltaTime;
            if (startGameTimer >= startGameTimerLength)
            {
                SceneManager.LoadScene("FightOne");
            }
        }
    }

    public void StartGame() 
    {
        flashPanel.SetActive(true);
        gameStarted = true;
    }

    public void CreditsScreen() 
    {
    
    }

    public void ExitGame() 
    {
        Application.Quit();
    }
}
