using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Or TMPro if using TextMeshPro
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] GameObject tipingSound;
    public TMP_Text myText; // Assign your Text UI element in the Inspector
    public string textToType;
    public float typingSpeed = 0.1f; // Adjust typing speed here
    int _tipingCounter;

    void Start()
    {
        if (myText == null)
        {
            Debug.LogError("Text component not assigned!");
            enabled = false; // Disable the script if no Text component is assigned.
            return;
        }
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in textToType.ToCharArray())
        {
            _tipingCounter++;
            if (_tipingCounter % 3 == 0)
            {
            }
            myText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}