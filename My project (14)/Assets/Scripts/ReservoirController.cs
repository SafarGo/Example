using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReservoirController : MonoBehaviour
{
    public GameObject imageMax;
    public string tag;
    [SerializeField] int maxHoneyCount;
    [SerializeField] int currentHuneyCount;
    [SerializeField] private TextMeshProUGUI honeyText;
    [SerializeField] private Transform HoneyFluid;


    private void Start()
    {
        UpdateHoneyText();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(tag))
        {
            if (currentHuneyCount < maxHoneyCount)
            {
                currentHuneyCount++;
                HoneyFluid.position += new Vector3(0, 0.035f, 0);
                imageMax.SetActive(false);
                UpdateHoneyText();
            }
            else
            {
                UpdateHoneyText();
                imageMax.SetActive(true);
            }


            Destroy(collision.gameObject);
            Debug.Log(currentHuneyCount);
        }
    }
    private void UpdateHoneyText()
    {
        if (honeyText != null)
        {
            honeyText.text = $"{currentHuneyCount}/{maxHoneyCount}";
        }
        //HoneyFluid.position += new Vector3(0,currentHuneyCount / 40,0);
    }

}
