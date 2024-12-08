using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;
    public Button nextButton;
    public Button cancelButton;
    public Image characterImage1;
    public Image characterImage2;
    public float typingSpeed = 0.05f;

    private List<Dialogue> dialogues;
    private int currentDialogueIndex = 0;
    private bool isTyping = false;

    [System.Serializable]
    public struct Dialogue
    {
        public string speaker;
        public string text;
        public Sprite characterSprite;
    }

    public Dialogue[] dialogueArray;

    private Dictionary<string, Image> speakerImages = new Dictionary<string, Image>();

    void Start()
    {
        if (dialogueArray == null || dialogueArray.Length == 0)
        {
            Debug.LogError("Dialogue array is empty or not assigned.");
            return;
        }
        dialogues = new List<Dialogue>(dialogueArray);
        if (nextButton == null)
        {
            Debug.LogError("Next button is not assigned.");
            return;
        }
        speakerImages.Add("Character 1", characterImage1);
        speakerImages.Add("Character 2", characterImage2);
        nextButton.onClick.AddListener(NextDialogue);
        nextButton.interactable = true; // Enable button initially

        //Initially hide both character images
        characterImage1.gameObject.SetActive(false);
        characterImage2.gameObject.SetActive(false);

        ShowDialogue();
    }

    void ShowDialogue()
    {
        if (isTyping) return;
        if (currentDialogueIndex < dialogues.Count)
        {
            Dialogue currentDialogue = dialogues[currentDialogueIndex];
            string speaker = currentDialogue.speaker;

            if (speakerImages.ContainsKey(speaker))
            {
                Image currentImage = speakerImages[speaker];
                currentImage.sprite = currentDialogue.characterSprite;
                currentImage.gameObject.SetActive(true);

                //Hide the other image ONLY if a speaker exists
                HideOtherImage(currentImage);
            }
            else
            {
                Debug.LogError($"Speaker '{speaker}' not found in speakerImages dictionary.");
            }
            StartCoroutine(TypeText(currentDialogue.text));
        }
        else
        {
            nextButton.gameObject.SetActive(false);
            characterImage1.gameObject.SetActive(false);
            characterImage2.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(true);
        }
    }

    void HideOtherImage(Image shownImage)
    {
        foreach (KeyValuePair<string, Image> kvp in speakerImages)
        {
            if (kvp.Value != shownImage)
            {
                kvp.Value.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        nextButton.interactable = false;
        dialogueText.text = "";
        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        nextButton.interactable = true;
    }

    void NextDialogue()
    {
        if (isTyping) return;
        currentDialogueIndex++;
        dialogueText.text = "";
        ShowDialogue();
    }
}