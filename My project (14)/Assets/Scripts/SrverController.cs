using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using static System.Net.WebRequestMethods;
using System;

public class SrverController : MonoBehaviour
{
    [SerializeField] private string gameName = "yourGameName"; // Укажите имя игры
    private string apiUrl = "https://2025.nti-gamedev.ru/api/games";

    public int currentServerSimpleHoney; // Значение, которое отправляем на сервер

    public void SendHoneyValue()
    {
        StartCoroutine(SendHoneyToServer());
    }

    private IEnumerator SendHoneyToServer()
    {
        // Формируем URL
        string url = $"{apiUrl}/{gameName}/players/bear1/";

        // Проверяем URL
        if (string.IsNullOrEmpty(gameName) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            Debug.LogError("Ошибка: Неверный URL. Проверьте значение gameName или apiUrl.");
            yield break;
        }

        // Формируем JSON-данные
        string jsonData = JsonUtility.ToJson(new { honey = currentServerSimpleHoney });

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            // Устанавливаем тело запроса
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Устанавливаем заголовок Content-Type
            request.SetRequestHeader("Content-Type", "application/json");

            // Отправляем запрос
            yield return request.SendWebRequest();

            // Проверяем статус ответа
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Успешно отправлено на сервер: " + request.downloadHandler.text);
            }
            else if (request.result == UnityWebRequest.Result.ConnectionError ||
                     request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Ошибка при отправке: {request.error}. Статус: {request.responseCode}");

                // Повторный запрос (если ошибка не из-за URL)
                if (request.responseCode != 400)
                {
                    Debug.LogWarning("Повторная попытка отправки...");
                    StartCoroutine(SendHoneyToServer());
                }
            }
            else
            {
                Debug.LogError("Неизвестная ошибка: " + request.error);
            }
        }
    }
}
