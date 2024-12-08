using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 

public class FlowersFresh : MonoBehaviour
{
    [SerializeField] private float freshment;
    [SerializeField] private Slider freshmentSlider;
    [SerializeField] private ParticleSystem water;
    private AudioSource waterSource;
    private static List<FlowersFresh> allFlowers = new List<FlowersFresh>(); // ������ ���� ��������
    private static bool isUpdaterRunning = false; // �������� ������� InvokeRepeating

    private void Start()
    {
        waterSource = gameObject.GetComponent<AudioSource>();
        freshment = 100;

        if (freshmentSlider != null)
        {
            freshmentSlider.maxValue = 100.0f;
            freshmentSlider.value = freshment;
        }

        // ��������� ������ � ������
        allFlowers.Add(this);

        // ��������, ��� ���������� �������� ������ ���� ���
        if (!isUpdaterRunning)
        {
            isUpdaterRunning = true;
            InvokeRepeating(nameof(CallUpdateGlobalFreshment), 10f, 10f);
        }
    }

    private void OnDestroy()
    {
        // ������� ������ �� ������ ��� ��� �����������
        allFlowers.Remove(this);

        // ���� ������ ����, ������������� ����������
        if (allFlowers.Count == 0)
        {
            isUpdaterRunning = false;
            CancelInvoke(nameof(CallUpdateGlobalFreshment));
        }
    }

    private void FixedUpdate()
    {
        // ���������� �������� freshment
        freshment -= Time.deltaTime / 3;
        freshment = Mathf.Clamp(freshment, 0, 100);

        if (freshmentSlider != null)
        {
            freshmentSlider.value = freshment;
        }
    }

    public void Refresher()
    {
        if (water != null)
        {
            water.Play();
        }

        freshment += Time.deltaTime * 7;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKey(KeyCode.E))
        {
            waterSource.mute = false;
            Refresher();
        }
        else
        {
            waterSource.mute = true;
        }
    }

    // ����� ��� ������ ���������� ����� InvokeRepeating
    private void CallUpdateGlobalFreshment()
    {
        UpdateGlobalFreshment();
    }

    // ����� ��� ���������� ������ freshment
    private void UpdateGlobalFreshment()
    {
        float totalFreshment = 0f;

        foreach (var flower in allFlowers)
        {
            totalFreshment += flower.freshment; // ��������� �������� ���� ��������
        }

        ClumbsManager.UpdateCounts(totalFreshment); // �������� ����� � ����������� �����
    }
}

