using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameMultiSlider : MonoBehaviour
{
    public Slider[] gameSliders;
    public float speed = 5f;
    public TextMeshProUGUI resultText;
    private int currentSliderIndex = 0;
    private float[] sliderValues;
    private bool gameStarted = false;
    private bool gameEnded = false;
    private bool movingRight = true;
    private float[] sliderMaxValues;
    private float[] targetZoneStartValues;
    private float[] targetZoneEndValues;
    private float[] targetZoneWidths;
    private bool[] sliderSuccesses;
    public Image targetZoneGraphicPrefab;
    private const float MIN_TARGET_WIDTH_PERCENT = 0.2f; // Minimum width (2x original)

    void Start()
    {
        int numSliders = gameSliders.Length;
        sliderMaxValues = new float[numSliders];
        targetZoneStartValues = new float[numSliders];
        targetZoneEndValues = new float[numSliders];
        targetZoneWidths = new float[numSliders];
        sliderValues = new float[numSliders];
        sliderSuccesses = new bool[numSliders];

        // Error Handling: Check for null or empty arrays before accessing them.
        if (gameSliders == null || gameSliders.Length == 0)
        {
            Debug.LogError("No sliders assigned to gameSliders array!");
            enabled = false;
            return;
        }
        if (targetZoneGraphicPrefab == null)
        {
            Debug.LogError("Target zone graphic prefab not assigned!");
            enabled = false;
            return;
        }
        if (resultText == null)
        {
            Debug.LogError("Result text UI element not assigned!");
            enabled = false;
            return;
        }

        for (int i = 0; i < numSliders; i++)
        {
            sliderMaxValues[i] = gameSliders[i].maxValue;
            GenerateUniqueTargetZone(i);
            sliderValues[i] = 0f;
        }

        CreateTargetZoneImages(); // Create images at the start
    }

    void Update()
    {
        if (gameStarted)
        {
            if (!gameEnded)
            {
                // Slider Movement Logic
                if (movingRight)
                {
                    sliderValues[currentSliderIndex] += Time.deltaTime * speed;
                    if (sliderValues[currentSliderIndex] >= sliderMaxValues[currentSliderIndex])
                    {
                        sliderValues[currentSliderIndex] = sliderMaxValues[currentSliderIndex];
                        movingRight = false;
                    }
                }
                else
                {
                    sliderValues[currentSliderIndex] -= Time.deltaTime * speed;
                    if (sliderValues[currentSliderIndex] <= 0f)
                    {
                        sliderValues[currentSliderIndex] = 0f;
                        movingRight = true;
                    }
                }
                gameSliders[currentSliderIndex].value = sliderValues[currentSliderIndex];

                // Check for Input
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
        float targetZoneStartPos = sliderMaxValues[currentSliderIndex] * targetZoneStartValues[currentSliderIndex];
        float targetZoneEndPos = targetZoneStartPos + targetZoneWidths[currentSliderIndex];

        if (sliderValues[currentSliderIndex] >= targetZoneStartPos && sliderValues[currentSliderIndex] <= targetZoneEndPos)
        {
            sliderSuccesses[currentSliderIndex] = true;
            currentSliderIndex++;
            if (currentSliderIndex >= gameSliders.Length)
            {
                gameEnded = true;
                resultText.text = "You Win!";
            }
            else
            {
                sliderValues[currentSliderIndex] = 0f;
                resultText.text = "Success! Next slider.";
            }
        }
        else
        {
            resultText.text = "Failed! Starting over.";
            RestartGame(); // Restart the entire game on any failure
        }
    }

    void RestartGame()
    {
        gameStarted = false;
        gameEnded = false;
        movingRight = true;
        currentSliderIndex = 0;

        for (int i = 0; i < gameSliders.Length; i++)
        {
            sliderSuccesses[i] = false;
            sliderValues[i] = 0f;
            gameSliders[i].value = 0f;
            DestroyImmediate(gameSliders[i].transform.Find("TargetZoneImage")?.gameObject);
        }

        CreateTargetZoneImages();
    }

    void GenerateUniqueTargetZone(int sliderIndex)
    {
        targetZoneStartValues[sliderIndex] = Random.Range(0f, 0.7f);
        targetZoneEndValues[sliderIndex] = Random.Range(targetZoneStartValues[sliderIndex] + 0.1f, 1f);

        // Ensure minimum target zone width
        float calculatedWidth = (targetZoneEndValues[sliderIndex] - targetZoneStartValues[sliderIndex]) * sliderMaxValues[sliderIndex];
        targetZoneWidths[sliderIndex] = Mathf.Max(calculatedWidth, MIN_TARGET_WIDTH_PERCENT * sliderMaxValues[sliderIndex]);
    }

    void CreateTargetZoneImages()
    {
        for (int i = 0; i < gameSliders.Length; i++)
        {
            CreateTargetZoneImage(i);
        }
    }

    void CreateTargetZoneImage(int sliderIndex)
    {
        Image newImage = Instantiate(targetZoneGraphicPrefab, gameSliders[sliderIndex].transform);
        newImage.name = "TargetZoneImage";
        RectTransform imageRect = newImage.GetComponent<RectTransform>();
        RectTransform sliderRect = gameSliders[sliderIndex].GetComponent<RectTransform>();

        if (imageRect != null && sliderRect != null)
        {
            float sliderWidth = sliderRect.rect.width;
            float targetZoneWidth = targetZoneWidths[sliderIndex];

            imageRect.anchorMin = new Vector2(targetZoneStartValues[sliderIndex], 0.5f);
            imageRect.anchorMax = new Vector2(targetZoneEndValues[sliderIndex], 0.5f);
            imageRect.sizeDelta = new Vector2(targetZoneWidth, 20f);
            imageRect.anchoredPosition = Vector2.zero;
            imageRect.SetSiblingIndex(0); // Draw behind other children
            imageRect.localPosition = new Vector3(imageRect.localPosition.x, imageRect.localPosition.y, -1f); // Adjust z as needed
        }
        else
        {
            Debug.LogError("Error creating target image. Check prefab and slider setup.");
        }
    }
}