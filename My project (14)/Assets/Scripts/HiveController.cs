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
    public Slider ProizvodSlider;
    public Slider Partiua;
    [SerializeField] GameObject honeyBottlePrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] int cartridgeCount;
    [SerializeField] int beesCount;
    [SerializeField] int currentHuneyCount;
    [SerializeField] int honeyCreateRate = 120;
    float t;

    private void Start()
    {
        UpdateBeesCount();
        InvokeRepeating(nameof(UpdateBeesCount), 0, 10);
        InvokeRepeating(nameof(UpdateTime), 0, 2);
        Time.timeScale = 1;
        HP = HPMax;
        sl.maxValue = HPMax;
        sl.value = HP;
        StaticHolder.HivesCount++;
        //honeyCreateRate = 12;// ������ ���� 120
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
            // �������� ����� ����������
            yield return new WaitForSeconds(0.5f); // ����� ��������� ������ �������� � ��������
        }
    }
    private void FixedUpdate()
    { 
        //Time.timeScale = 5;
        UpdateHoneyText();
        t += Time.deltaTime;
        if (t > honeyCreateRate)
        {
            t = 0;
            StartCoroutine(HoneyCreatingCoroutine());
        }
        //Debug.Log(t);
    }

    void UpdateBeesCount()
    {
        if (honeyCreateRate >= 15)                                                                                        //����� �� ������������ �������� �������� ����
        {                                                                                                                 //����� �� ������������ �������� �������� ����
            honeyCreateRate = 22 - System.Convert.ToInt32(ClumbsManager.FlowerCounts / (15 + StaticHolder.HivesCount));    //����� �� ������������ �������� �������� ����
            Debug.Log(honeyCreateRate);                                                                                   //����� �� ������������ �������� �������� ����
            ProizvodSlider.value = -honeyCreateRate / cartridgeCount;                                                     //����� �� ������������ �������� �������� ����
        }                                                                                                                 //����� �� ������������ �������� �������� ����
        else                                                                                                              //����� �� ������������ �������� �������� ����
        {                                                                                                                 //����� �� ������������ �������� �������� ����
            honeyCreateRate = 15;                                                                                         //����� �� ������������ �������� �������� ����
        }
        if (ClumbsManager.FlowerCounts == 0)//����� �� ������������ �������� �������� ����

        {
            honeyCreateRate = 15;
        }                                       //����� �� ������������ �������� �������� ����
    }                                                                                                                     //����� �� ������������ �������� �������� ����
    private void UpdateHoneyText()
    {
        if (honeyText != null)
        {
            honeyText.text = $"��������� + {cartridgeCount}";
        }
    }

    void UpdateTime()
    {
        Partiua.value = t / honeyCreateRate;
    }


}
