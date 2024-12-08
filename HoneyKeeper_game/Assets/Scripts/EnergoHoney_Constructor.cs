using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergoHoney_Constructor : MonoBehaviour
{
    [HideInInspector]public int HP;
    public int HPMax;
    public Slider HpSlider;
    [SerializeField] private TextMeshProUGUI catrText;
    public int honeyValue = 2;
    public GameObject energyHoneyPrefab;
    [SerializeField] int HoneyDelSpeed;
    public int medCaunter = 0;
    [SerializeField] int cartridgeCount = 1;
    public string honeyTag = "Honey";
    [SerializeField] private TextMeshProUGUI honeyText;
    [SerializeField] private Slider honeySlider;

    private Collider spawnerCollider; // Объявляем переменную для коллайдера

    private void Start()
    {
        HP = HPMax;
        HpSlider.maxValue = HPMax;
        HpSlider.value = HP;

        spawnerCollider = GetComponent<Collider>(); // Получаем коллайдер
        UpdateHoneyText();
        honeySlider.maxValue = HoneyDelSpeed / cartridgeCount;
        honeySlider.value = 0;
    }
    private void Update()
    {
        UpdateCatrText();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(honeyTag))
        {
            Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
            Vector3 velocity = otherRigidbody != null ? otherRigidbody.velocity : Vector3.zero;
            Destroy(other.gameObject);
            medCaunter++;
            UpdateHoneyText();

            if (medCaunter >= honeyValue)
            {
                StartCoroutine(SpawnEnergyHoney(other.transform.position, velocity));
                HP--;
                HpSlider.value = Mathf.Clamp(HP, 0f, HPMax);
                if (HP <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private IEnumerator SpawnEnergyHoney(Vector3 position, Vector3 velocity)
    {
        // Выключаем триггер
        spawnerCollider.isTrigger = false;

        float waitTime = HoneyDelSpeed / cartridgeCount;
        honeySlider.maxValue = waitTime;
        honeySlider.value = 0;

        // Процесс ожидания
        for (float elapsed = 0; elapsed < waitTime; elapsed += Time.deltaTime)
        {
            honeySlider.value = elapsed;
            yield return null;
        }

        GameObject newEnergyHoney = Instantiate(energyHoneyPrefab, position, Quaternion.identity);
        Rigidbody newHoneyRigidbody = newEnergyHoney.GetComponent<Rigidbody>();

        if (newHoneyRigidbody != null)
        {
            newHoneyRigidbody.velocity = velocity;
        }

        // Уменьшаем medCaunter, но не позволяем ему стать отрицательным
        medCaunter = Mathf.Max(medCaunter - honeyValue, 0); // Устанавливаем medCaunter в 0, если он меньше 0
        UpdateCatrText();
        honeySlider.value = 0;

        // Включаем триггер обратно
        spawnerCollider.isTrigger = true;
        UpdateHoneyText();
    }

    private void UpdateHoneyText()
    {
        if (honeyText != null)
        {
            honeyText.text = $"{medCaunter}/{honeyValue}";
        }
    }
    private void UpdateCatrText()
    {
        if (honeyText != null)
        {
            catrText.text = $"+{cartridgeCount}";
        }
    }

}
