using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalimboPlay : MonoBehaviour
{
    public List <AudioClip> SpawnedSound; // Массив объектов для спауна
    public List <Transform> Palochky; // Массив объектов для спауна
    public GameObject SpawnedNote;
    AudioSource _spawnedNote;
    bool isCalimboOn = false;
    [Header("Персонаж и камера")]
    [SerializeField]GameObject PLayer;
    [SerializeField]GameObject CalimboCamera;
    [SerializeField]GameObject CarCamera;
    [SerializeField]GameObject Music;
    private Dictionary<Transform, Coroutine> activeRotations = new Dictionary<Transform, Coroutine>();
    private KeyCode[] spawnKeys = {
        KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y,
        KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, KeyCode.G, KeyCode.H,
        KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V, KeyCode.B
    };


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            isCalimboOn = !isCalimboOn;
            StaticHolder.isCanOpenUI = !isCalimboOn;
            PLayer.SetActive(!isCalimboOn);
            Music.SetActive(!isCalimboOn);
            CalimboCamera.SetActive(isCalimboOn);
            if(isCalimboOn)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        if (isCalimboOn)
        {
            PlayCalimbo();
        }
    }

    public void SpawnObject(int objectNumber)
    {
        // Проверяем, чтобы индекс объекта был в пределах массива
        if (objectNumber >= 0 && objectNumber <= SpawnedSound.Count)
        {
            _spawnedNote =  Instantiate(SpawnedNote, transform.position, Quaternion.identity).GetComponent<AudioSource>();
            _spawnedNote.clip = SpawnedSound[objectNumber - 1];
            RotatePalochka(objectNumber);
            _spawnedNote.Play();
        }
        else
        {
            Debug.LogWarning("Неверный номер объекта");
        }
    }

   void PlayCalimbo()
   {
        // Пройдем по всем клавишам и спауним объект, если клавиша была нажата
        for (int i = 0; i < spawnKeys.Length; i++)
        {
            if (Input.GetKeyDown(spawnKeys[i]))
            {
                SpawnObject(i + 1);
                break;
            }
        }
    }

    void TurnOnKalimbo()
    {

    }
    void RotatePalochka(int palochaID)
    {
        // Проверяем, чтобы индекс палочки был в пределах массива
        if (palochaID >= 1 && palochaID <= Palochky.Count)
        {
            Transform palochka = Palochky[palochaID - 1];

            // Если корутина уже запущена для этой палочки
            if (activeRotations.ContainsKey(palochka) && activeRotations[palochka] != null)
            {
                // Останавливаем текущую корутину
                StopCoroutine(activeRotations[palochka]);

                // Немедленно возвращаем палочку в исходное положение
                palochka.localRotation = Quaternion.identity;
            }

            // Запускаем новую корутину и сохраняем её
            activeRotations[palochka] = StartCoroutine(RotatePalochkaCoroutine(palochka));
        }
        else
        {
            Debug.LogWarning("Неверный ID палочки");
        }
    }

    IEnumerator RotatePalochkaCoroutine(Transform palochka)
    {
        float rotationAngle = 5f; // Угол наклона
        float rotationSpeed = 10f; // Скорость поворота

        // Запоминаем начальный угол
        Quaternion initialRotation = Quaternion.identity;

        // Целевой угол (наклон вперед по оси X)
        Quaternion targetRotation = initialRotation * Quaternion.Euler(rotationAngle, 0, 0);

        // Плавный поворот вперед
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            palochka.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime);
            elapsedTime += Time.deltaTime * rotationSpeed;
            yield return null;
        }

        // Убедимся, что палочка точно в целевом положении
        palochka.localRotation = targetRotation;

        // Плавный возврат в начальное положение
        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            palochka.localRotation = Quaternion.Slerp(targetRotation, initialRotation, elapsedTime);
            elapsedTime += Time.deltaTime * rotationSpeed;
            yield return null;
        }

        // Убедимся, что палочка точно в начальном положении
        palochka.localRotation = initialRotation;

        // Убираем корутину из списка активных
        activeRotations[palochka] = null;
    }
}
