using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;
    public Button nextButton;
    public Image characterImage;
    public Sprite character1Sprite;
    public Sprite character2Sprite;
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

    void Start()
    {
        if (dialogueArray == null || dialogueArray.Length == 0)
        {
            Debug.LogError("Массив диалогов пуст или не задан.");
            return;
        }
        dialogues = new List<Dialogue>(dialogueArray);
        if (nextButton == null)
        {
            Debug.LogError("Кнопка \"Далее\" не назначена.");
            return;
        }
        nextButton.onClick.AddListener(NextDialogue);
        nextButton.interactable = !isTyping;
        ShowDialogue();
    }

    void ShowDialogue()
    {
        if (isTyping) return;
        if (currentDialogueIndex < dialogues.Count)
        {
            Dialogue currentDialogue = dialogues[currentDialogueIndex];
            characterImage.sprite = currentDialogue.characterSprite;
            StartCoroutine(TypeText(currentDialogue.text));
        }
        else
        {
            nextButton.gameObject.SetActive(false);
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