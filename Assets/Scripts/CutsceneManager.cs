using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class CutsceneManager : MonoBehaviour
{

    public MinigameManager minigameManager;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI dialogueTextElement;
    public Image dialogueBox;
    public Image leftSprite;
    public Image rightSprite;
    private Color32 fadeColor = new Color32(52, 52, 52, 255);
    private Color32 highlightColor = new Color32(255, 255, 255, 255);
    public float fadeSpeed = 0.008f;
    private float dialogueOpacity = 0f;
    private bool runningDialogue = true;
    private bool dialogueTyped = false;
    private bool dialogueEnded = false;
    private int dialogueIndex = 0;
    private int dialogueLineCharactersTyped = 0;
    private int endOfDialogue;
    private int dialogueTimer = 0;
    public int dialogueSlowness = 5;
    public int sceneIndex = 0;

    private string[,] dialogue;

    private string[,] sceneOne =
    {
        {"", "THE FIRST REUNION"},
        {"Right", "I couldn’t let you go, Elias"},
        {"Left", "You should’ve. Love ain’t supposed to rot like this."}
    };

    private string[,] sceneTwo =
    {
        {"", "FUNERAL FOR THE LIVING"},
        {"Right", "Back again, boy? Dirt ain’t done with you yet..."},
        {"Left", "We'll see."}
    };

    private string[,] sceneThree =
    {
        {"", "THE HOLLOWAY DEBT"},
        {"Right", "You died owing me blood, Elias. Don’t think death clears the tab."},
        {"Left", "..."}
    };

    private string[,] sceneFour =
    {
        {"", "BLEED THE RED GHOST"},
        {"Right", "Turn back."},
        {"Left", "I won't be doin' that, thanks." },
        {"Left", "Let’s see if blood still burns hotter than the sun on the sand."}
    };

    private string[,] sceneFive =
    {
        {"", "THUNDER AT THE END OF THE WORLD"},
        {"Right", "..."},
        {"Left", "There ain’t no heaven left to crawl to. Guess I’ll make my own one outta lightning."}
    };

    private string[,] sceneSix =
    {
        {"", "THE BOY WHO DREW TOO SLOW"},
        {"Right", "Used to polish your gun, Mister Pale. Used to dream I’d be half as fast."},
        {"Left", "Dream smaller next time, kid."}
    };

    private string[,] sceneTest =
    {
        {"Right", "Hey you big baddie zombie im gonna kill you"},
        {"Elias", "Nuh uhn"},
        {"Right", "Yuh huh buddy youre a stinky little bozo and I'm gonna shoot you with my funny gun"},
        {"Left", "Nuh uhn nuh uhn nuh uhn"}
    };


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SelectScene();

        endOfDialogue = dialogue.GetLength(0) - 1;
    }

    // Update is called once per frame
    void Update()
    {

        if (runningDialogue)
        {
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!dialogueTyped)
                {
                    dialogueLineCharactersTyped = dialogue[dialogueIndex, 1].Length;
                    dialogueTyped = true;
                }
                else
                {
                    if (dialogueIndex < endOfDialogue)
                    {
                        dialogueIndex++;
                        dialogueLineCharactersTyped = 0;
                        dialogueTyped = false;
                        //dialogueOpacity = 0f;
                    }
                    else
                    {
                        runningDialogue = false;
                    }
                }
            }

            if (dialogueOpacity < 1f) dialogueOpacity += fadeSpeed;
            canvasGroup.alpha = dialogueOpacity;

            dialogueTimer++;

            if (dialogueTimer >= dialogueSlowness)
            {
                IncrementDialogue();
                dialogueTimer = 0;
            }

            FillTextBox();
            
        }
        else 
        {
            if (dialogueOpacity > 0f)
            {
                dialogueOpacity -= fadeSpeed;
                canvasGroup.alpha = dialogueOpacity;
            }
            else 
            {
                if (!dialogueEnded) 
                {
                    dialogueOpacity = 0f;
                    canvasGroup.gameObject.SetActive(false);
                    runningDialogue = false;
                    minigameManager.StartMinigame();
                }
            }
        }
    }

    void FillTextBox() 
    {
        if (dialogueIndex == 0)
        {
            dialogueBox.color = Color.black;
            dialogueTextElement.color = Color.white;
        }
        else 
        {
            dialogueBox.color = Color.white;
            dialogueTextElement.color = Color.black;
        }

            string dialogueToDisplay = dialogue[dialogueIndex, 1].Substring(0, Math.Min(dialogueLineCharactersTyped, dialogue[dialogueIndex, 1].Length));

        dialogueTextElement.text = dialogueToDisplay;

        if (dialogue[dialogueIndex, 0] == "Left" || dialogue[dialogueIndex, 0] == "Elias")
        {
            leftSprite.color = highlightColor;
            rightSprite.color = fadeColor;
            leftSprite.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            rightSprite.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else if (dialogue[dialogueIndex, 0] == "Right" || dialogue[dialogueIndex, 0] == "Other")
        {
            rightSprite.color = highlightColor;
            leftSprite.color = fadeColor;
            rightSprite.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            leftSprite.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            rightSprite.color = fadeColor;
            leftSprite.color = fadeColor;
            leftSprite.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            rightSprite.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    void IncrementDialogue() 
    {
        if (dialogueLineCharactersTyped != dialogue[dialogueIndex, 1].Length)
        {
            dialogueLineCharactersTyped = Math.Min(dialogueLineCharactersTyped + 1, dialogue[dialogueIndex, 1].Length);
        }
        else 
        {
            dialogueTyped = true;
        }
    }

    void SelectScene() 
    {
        switch (sceneIndex) 
        {
            case 1:
                dialogue = sceneOne;
                break;
            case 2:
                dialogue = sceneTwo;
                break;
            case 3:
                dialogue = sceneThree;
                break;
            case 4:
                dialogue = sceneFour;
                break;
            case 5:
                dialogue = sceneFive;
                break;
            case 6:
                dialogue = sceneSix;
                break;
            default:
                dialogue = sceneTest;
                break;
        }
    }
}
