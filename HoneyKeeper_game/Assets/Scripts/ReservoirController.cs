using System.Collections;
using TMPro;
using UnityEngine;

public class ReservoirController : MonoBehaviour
{
    public static ReservoirController instance_honey { get; private set; }
    public static ReservoirController instance_energo { get; private set; }
    public GameObject imageMax;
    public string tag;
    [SerializeField] int maxHoneyCount;
    public int currentHuneyCount = 3;
    public TMP_Text honeyText;
    //private TMP_Text allHoneyText;
    //private TMP_Text allEnergyHoneyText;
    [SerializeField] public Transform HoneyFluid;
    public bool isEnergoHoneyRzervoir;


    // Максимальное значение
    public float maxValue = 100f;

    // Минимальная высота
    public float minHeight = 0f;

    // Максимальная высота
    public float maxHeight = 10f;


    private void Awake()
    {
    }

    private void Start()
    {
        JsonSaver._instance.Load();
        if (isEnergoHoneyRzervoir)
        {
            instance_energo = this;
        }
        else
        {
            instance_honey = this;
        }

        if (isEnergoHoneyRzervoir)
        {
            currentHuneyCount = StaticHolder.count_of_simple_honey;
        }
        else
        {
            currentHuneyCount = StaticHolder.count_of_enegry_honey;
        }
        //allHoneyText = GameObject.Find("AllHoney_Text").GetComponent<TMP_Text>();
        //allEnergyHoneyText = GameObject.Find("AllEnergyHoney_Text").GetComponent<TMP_Text>();
        Debug.Log("Первая ли игра - " + StaticHolder.isFirstGame);
        UpdateHoneyText();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(tag))
        {
            Destroy(collision.gameObject);
            if (currentHuneyCount < maxHoneyCount)
            {
                currentHuneyCount++;
                SetHeightByValue(currentHuneyCount);
                imageMax.SetActive(false);
            }
            else
            {
                imageMax.SetActive(true);
            }
            UpdateHoneyText();
        }
    }

    public void UpdateHoneyText()
    {

        //IsEndGameController.instance.CheckEndingGame();
        //if (honeyText != null)
        //{
            honeyText.text = $"{currentHuneyCount}/{maxHoneyCount}";
        //}
        if(isEnergoHoneyRzervoir)
        {
            SrverController.instance.EnergyHoney = currentHuneyCount.ToString();
            StaticHolder.count_of_enegry_honey = currentHuneyCount;
        }
        else
        {
            SrverController.instance.SimpleHoney = currentHuneyCount.ToString();
            StaticHolder.count_of_simple_honey = currentHuneyCount;
        }
        SrverController.instance.SendPutRequest();
        Debug.Log(StaticHolder.count_of_simple_honey);
        JsonSaver._instance.Save();
        //int simpleHoneyTotal = 0;
        //int energyHoneyTotal = 0;
        //
        //foreach (var reservoir in FindObjectsOfType<ReservoirController>())
        //{
        //    if (reservoir.isEnergoHoneyRzervoir)
        //    {
        //        energyHoneyTotal += reservoir.currentHuneyCount;
        //    }
        //    else
        //    {
        //        simpleHoneyTotal += reservoir.currentHuneyCount;
        //    }
        //}
        //
        //StaticHolder.count_of_simple_honey = simpleHoneyTotal;
        //StaticHolder.count_of_enegry_honey = energyHoneyTotal;
        //
        //if (allHoneyText != null)
        //{
        //    allHoneyText.text = StaticHolder.count_of_simple_honey.ToString();
        //}
        //if (allEnergyHoneyText != null)
        //{
        //    allEnergyHoneyText.text = StaticHolder.count_of_enegry_honey.ToString();
        //}
    }
    // Установить высоту объекта в зависимости от числа
    // Установить положение объекта в зависимости от числа
    public void SetHeightByValue(float value)
    {
        if (maxValue <= 0)
        {
            Debug.LogError("Максимальное значение должно быть больше 0.");
            return;
        }

        // Рассчитать пропорцию значения в диапазоне [0, 1]
        float proportionalValue = Mathf.Clamp01(value / maxValue);

        // Рассчитать новую высоту в диапазоне от minHeight до maxHeight
        float newY = Mathf.Lerp(minHeight, maxHeight, proportionalValue);

        // Получить текущее положение объекта
        Vector3 position = transform.position;

        // Установить новое положение по оси Y
        position.y = newY;

        // Применить новое положение
        HoneyFluid.transform.position = position;
    }
}
