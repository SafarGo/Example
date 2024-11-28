using System.Collections;
using TMPro;
using UnityEngine;

public class ReservoirController : MonoBehaviour
{
    public static ReservoirController instance { get; private set; }
    public GameObject imageMax;
    public string tag;
    [SerializeField] int maxHoneyCount;
    public int currentHuneyCount;
    [SerializeField] private TMP_Text honeyText;
    private TMP_Text allHoneyText;
    private TMP_Text allEnergyHoneyText;
    [SerializeField] private Transform HoneyFluid;
    public bool isEnergoHoneyRzervoir;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        allHoneyText = GameObject.Find("AllHoney_Text").GetComponent<TMP_Text>();
        allEnergyHoneyText = GameObject.Find("AllEnergyHoney_Text").GetComponent<TMP_Text>();
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
                HoneyFluid.position += new Vector3(0, 0.015f, 0);
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
        if (honeyText != null)
        {
            honeyText.text = $"{currentHuneyCount}/{maxHoneyCount}";
        }

        int simpleHoneyTotal = 0;
        int energyHoneyTotal = 0;

        foreach (var reservoir in FindObjectsOfType<ReservoirController>())
        {
            if (reservoir.isEnergoHoneyRzervoir)
            {
                energyHoneyTotal += reservoir.currentHuneyCount;
            }
            else
            {
                simpleHoneyTotal += reservoir.currentHuneyCount;
            }
        }

        StaticHolder.count_of_simple_honey = simpleHoneyTotal;
        StaticHolder.count_of_enegry_honey = energyHoneyTotal;

        if (allHoneyText != null)
        {
            allHoneyText.text = StaticHolder.count_of_simple_honey.ToString();
        }
        if (allEnergyHoneyText != null)
        {
            allEnergyHoneyText.text = StaticHolder.count_of_enegry_honey.ToString();
        }
    }
}
