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
    private static List<FlowersFresh> allFlowers = new List<FlowersFresh>(); // Список всех объектов
    private static bool isUpdaterRunning = false; // Контроль запуска InvokeRepeating

    private void Start()
    {
        waterSource = gameObject.GetComponent<AudioSource>();
        freshment = 100;

        if (freshmentSlider != null)
        {
            freshmentSlider.maxValue = 100.0f;
            freshmentSlider.value = freshment;
        }

        // Добавляем объект в список
        allFlowers.Add(this);

        // Убедимся, что обновление запущено только один раз
        if (!isUpdaterRunning)
        {
            isUpdaterRunning = true;
            InvokeRepeating(nameof(CallUpdateGlobalFreshment), 10f, 10f);
        }
    }

    private void OnDestroy()
    {
        // Удаляем объект из списка при его уничтожении
        allFlowers.Remove(this);

        // Если список пуст, останавливаем обновление
        if (allFlowers.Count == 0)
        {
            isUpdaterRunning = false;
            CancelInvoke(nameof(CallUpdateGlobalFreshment));
        }
    }

    private void FixedUpdate()
    {
        // Уменьшение значения freshment
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

    // Метод для вызова обновления через InvokeRepeating
    private void CallUpdateGlobalFreshment()
    {
        UpdateGlobalFreshment();
    }

    // Метод для обновления общего freshment
    private void UpdateGlobalFreshment()
    {
        float totalFreshment = 0f;

        foreach (var flower in allFlowers)
        {
            totalFreshment += flower.freshment; // Суммируем значения всех объектов
        }

        ClumbsManager.UpdateCounts(totalFreshment); // Передаем сумму в статический класс
    }
}

