using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HiveController : MonoBehaviour
{
    int HP;
   public int HPMax;
    [SerializeField] private TextMeshProUGUI honeyText;

    public Slider sl;
    [SerializeField] GameObject honeyBottlePrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] int cartridgeCount;
    [SerializeField] int beesCount;
    [SerializeField] int currentHuneyCount;
    [SerializeField] int honeyCreateRate = 120;
    float t;

    private void Start()
    {


        Time.timeScale = 1;
        HP = HPMax;
        sl.maxValue = HPMax;
        sl.value = HP;

        //honeyCreateRate = 12;// Должно быть 120
    }
    private IEnumerator HoneyCreatingCoroutine()
    {
        for (int i = 0; i < cartridgeCount; i++)
        {
            Instantiate(honeyBottlePrefab, spawnPoint.position, Quaternion.identity);
            HP--;
            sl.value = Mathf.Clamp(HP, 0f, HPMax);
            if (HP <= 0)
            {
                Destroy(gameObject);
            }
            // Задержка между созданиями
            yield return new WaitForSeconds(0.5f); // Здесь указываем нужную задержку в секундах
        }
    }
    private void FixedUpdate()
    {
        UpdateHoneyText();
        t += Time.deltaTime;
        if (t > honeyCreateRate)
        {
            t = 0;
            StartCoroutine(HoneyCreatingCoroutine());
        }
        //Debug.Log(t);
    }

    void UpdateBeesCount(int plusBees)
    {
        if (beesCount % 2 == 0)
        {
            honeyCreateRate = 120 / beesCount;
        }

    }
    private void UpdateHoneyText()
    {
        if (honeyText != null)
        {
            honeyText.text = $"+{cartridgeCount}";
        }
    }

}
