using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class CutsceneManager : MonoBehaviour
{

    public MinigameManager minigameManager;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI dialogueTextElement;
    public GameObject dialogueBox;
    public Image leftSprite;
    public Image rightSprite;
    private Color32 fadeColor = new Color32(52, 52, 52, 255);
    private Color32 highlightColor = new Color32(255, 255, 255, 255);
    public float fadeSpeed = 0.002f;
    private float dialogueOpacity = 0f;
    private bool runningDialogue = true;
    private bool dialogueTyped = false;
    private bool dialogueEnded = false;
    private int dialogueIndex = 0;
    private int dialogueLineCharactersTyped = 0;
    private int endOfDialogue;
    private int dialogueTimer = 0;
    public int dialogueSlowness = 5;

    private string[,] dialogue =
    {
        {"Right", "Hey you big baddie zombie im gonna kill you"},
        {"Left", "Nuh uhn"},
        {"Right", "Yuh huh buddy youre a stinky little bozo and I'm gonna shoot you with my funny gun"},
        {"Left", "Nuh uhn nuh uhn nuh uhn"}
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

            //Color boxColor = canvasGroup.GetComponent<Image>().color;
            //boxColor.a = Math.Min(boxColor.a + 0.01f, 1f);
            //dialogueBox.GetComponent<Image>().color = boxColor;


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
                    dialogueBox.SetActive(false);
                    canvasGroup.gameObject.SetActive(false);
                    runningDialogue = false;
                    minigameManager.StartMinigame();
                }
            }
        }
    }

    void FillTextBox() 
    {
        string dialogueToDisplay = dialogue[dialogueIndex, 1].Substring(0, Math.Min(dialogueLineCharactersTyped, dialogue[dialogueIndex, 1].Length));

        dialogueTextElement.text = dialogueToDisplay;

        if (dialogue[dialogueIndex, 0] == "Left")
        {
            leftSprite.color = highlightColor;
            rightSprite.color = fadeColor;
            leftSprite.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            rightSprite.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else if (dialogue[dialogueIndex, 0] == "Right")
        {
            rightSprite.color = highlightColor;
            leftSprite.color = fadeColor;
            rightSprite.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            leftSprite.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            rightSprite.color = highlightColor;
            leftSprite.color = highlightColor;
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
}
