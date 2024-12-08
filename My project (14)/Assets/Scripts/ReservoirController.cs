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


    // ������������ ��������
    public float maxValue = 100f;

    // ����������� ������
    public float minHeight = 0f;

    // ������������ ������
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
        Debug.Log("������ �� ���� - " + StaticHolder.isFirstGame);
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
    // ���������� ������ ������� � ����������� �� �����
    // ���������� ��������� ������� � ����������� �� �����
    public void SetHeightByValue(float value)
    {
        if (maxValue <= 0)
        {
            Debug.LogError("������������ �������� ������ ���� ������ 0.");
            return;
        }

        // ���������� ��������� �������� � ��������� [0, 1]
        float proportionalValue = Mathf.Clamp01(value / maxValue);

        // ���������� ����� ������ � ��������� �� minHeight �� maxHeight
        float newY = Mathf.Lerp(minHeight, maxHeight, proportionalValue);

        // �������� ������� ��������� �������
        Vector3 position = transform.position;

        // ���������� ����� ��������� �� ��� Y
        position.y = newY;

        // ��������� ����� ���������
        HoneyFluid.transform.position = position;
    }
}
