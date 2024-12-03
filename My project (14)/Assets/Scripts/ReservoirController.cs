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

    private void Start()
    {
        if(isEnergoHoneyRzervoir)
        {
            instance_energo = this;
        }
        else
        {
            instance_honey = this;
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

        IsEndGameController.instance.CheckEndingGame();
        //if (honeyText != null)
        //{
            honeyText.text = $"{currentHuneyCount}/{maxHoneyCount}";
        //}
        if(isEnergoHoneyRzervoir)
        {
            StaticHolder.count_of_enegry_honey = currentHuneyCount;
        }
        else
        {
            StaticHolder.count_of_simple_honey = currentHuneyCount;
        }
        Debug.Log(StaticHolder.count_of_simple_honey);

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
}
