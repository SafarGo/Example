using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameMultiSlider : MonoBehaviour
{
    public Slider[] gameSliders; // Array to hold multiple sliders
    public float speed = 5f;
    public float targetZoneStart = 1f / 3f;
    public float targetZoneEnd = 2f / 3f;
    public TextMeshProUGUI resultText;
    private int currentSliderIndex = 0; // Index of the currently active slider
    private float sliderValue;
    private bool gameStarted = false;
    private bool gameEnded = false;
    private bool movingRight = true;
    private float[] sliderMaxValues;
    private float[] targetZoneWidths;
    private bool[] sliderSuccesses; //Added to track success of each slider


    void Start()
    {
        // Error handling
        if (targetZoneStart < 0f || targetZoneStart > 1f || targetZoneEnd < 0f || targetZoneEnd > 1f || targetZoneStart >= targetZoneEnd)
        {
            Debug.LogError("Invalid target zone values.");
            enabled = false;
            return;
        }

        int numSliders = gameSliders.Length;
        sliderMaxValues = new float[numSliders];
        targetZoneWidths = new float[numSliders];
        sliderSuccesses = new bool[numSliders]; // Initialize to all false initially

        for (int i = 0; i < numSliders; i++)
        {
            sliderMaxValues[i] = gameSliders[i].maxValue;
            targetZoneWidths[i] = sliderMaxValues[i] * (targetZoneEnd - targetZoneStart);
        }

        RestartGame();
    }

    void Update()
    {
        if (gameStarted)
        {
            if (!gameEnded)
            {
                // Movement logic for the current slider
                if (movingRight)
                {
                    sliderValue += Time.deltaTime * speed;
                    if (sliderValue >= sliderMaxValues[currentSliderIndex])
                    {
                        sliderValue = sliderMaxValues[currentSliderIndex];
                        movingRight = false;
                    }
                }
                else
                {
                    sliderValue -= Time.deltaTime * speed;
                    if (sliderValue <= 0f)
                    {
                        sliderValue = 0f;
                        movingRight = true;
                    }
                }

                gameSliders[currentSliderIndex].value = sliderValue;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    CheckResult();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    RestartGame();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            gameStarted = true;
        }
    }

    void CheckResult()
    {
        float targetZoneStartPos = sliderMaxValues[currentSliderIndex] * targetZoneStart;
        float targetZoneEndPos = targetZoneStartPos + targetZoneWidths[currentSliderIndex];

        if (sliderValue >= targetZoneStartPos && sliderValue <= targetZoneEndPos)
        {
            sliderSuccesses[currentSliderIndex] = true; // Mark current slider as successful
            currentSliderIndex++;

            if (currentSliderIndex >= gameSliders.Length)
            {
                gameEnded = true;
                resultText.text = "You Win!";
            }
            else
            {
                sliderValue = 0f;
                gameEnded = false;
                resultText.text = "Success! Next slider.";
            }

        }
        else
        {
            gameEnded = true;
            resultText.text = "Failed!";
            RestartGame();
        }
    }

    void RestartGame()
    {
        gameStarted = false;
        gameEnded = false;
        movingRight = true;
        sliderValue = 0f;
        resultText.text = "";
        currentSliderIndex = 0;
        for (int i = 0; i < sliderSuccesses.Length; i++)
        {
            sliderSuccesses[i] = false;
        }

        for (int i = 0; i < gameSliders.Length; i++)
        {
            gameSliders[i].value = 0f;
        }
    }
}