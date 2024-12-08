using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
public class SrverController : MonoBehaviour
{
    private string baseUrl = "https://2025.nti-gamedev.ru/api/games/bb62ba54-dcaa-4097-89b3-903938dffa60/players/";
    public static SrverController instance;

    // Динамическое имя игрока
    public string playerName = "bear1";

    // Значения для отправки
    public string SimpleHoney = "0";
    public string EnergyHoney = "0";

    // Метод для отправки данных
    public void SendPutRequest()
    {
        StartCoroutine(SendRequestCoroutine());
    }

    private IEnumerator SendRequestCoroutine()
    {
        // Создаем JSON-объект для отправки
        string jsonBody = JsonUtility.ToJson(new RequestData
        {
            name = playerName,
            resources = new ResourceData
            {
                SimpleHoney = this.SimpleHoney,
                EnergyHoney = this.EnergyHoney
            }
        });

        // Формируем URL на основе имени игрока
        string url = baseUrl + playerName + "/";

        // Настраиваем PUT-запрос
        UnityWebRequest request = new UnityWebRequest(url, "PUT");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Отправка запроса
        yield return request.SendWebRequest();

        // Обработка ответа
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Request successful for {playerName}. Response: {request.downloadHandler.text}");
        }
        else
        {
            Debug.LogError($"Request failed for {playerName}. Error: {request.error}\nResponse Code: {request.responseCode}");
        }
    }

    // Тестовый вызов при старте
    private void Start()
    {
        JsonSaver._instance.Load();
        SimpleHoney = StaticHolder.count_of_simple_honey.ToString();
        EnergyHoney = StaticHolder.count_of_enegry_honey.ToString();

        // Динамическое определение имени игрока (например, можно задавать из другого скрипта)
        playerName = PlayerPrefs.GetString("PlayerName", "bear1"); // Загружаем имя игрока или используем "bear1" по умолчанию

        instance = this;
        SendPutRequest();
    }

    // Классы для сериализации JSON
    [Serializable]
    private class RequestData
    {
        public string name;
        public ResourceData resources;
    }

    [Serializable]
    private class ResourceData
    {
        public string SimpleHoney;
        public string EnergyHoney;
    }
}
