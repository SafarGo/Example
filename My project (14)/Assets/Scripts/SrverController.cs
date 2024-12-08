using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using static System.Net.WebRequestMethods;
using System;

public class SrverController : MonoBehaviour
{
    [SerializeField] private string gameName = "yourGameName"; // ������� ��� ����
    private string apiUrl = "https://2025.nti-gamedev.ru/api/games";

    public int currentServerSimpleHoney; // ��������, ������� ���������� �� ������

    public void SendHoneyValue()
    {
        StartCoroutine(SendHoneyToServer());
    }

    private IEnumerator SendHoneyToServer()
    {
        // ��������� URL
        string url = $"{apiUrl}/{gameName}/players/bear1/";

        // ��������� URL
        if (string.IsNullOrEmpty(gameName) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            Debug.LogError("������: �������� URL. ��������� �������� gameName ��� apiUrl.");
            yield break;
        }

        // ��������� JSON-������
        string jsonData = JsonUtility.ToJson(new { honey = currentServerSimpleHoney });

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            // ������������� ���� �������
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();

            // ������������� ��������� Content-Type
            request.SetRequestHeader("Content-Type", "application/json");

            // ���������� ������
            yield return request.SendWebRequest();

            // ��������� ������ ������
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("������� ���������� �� ������: " + request.downloadHandler.text);
            }
            else if (request.result == UnityWebRequest.Result.ConnectionError ||
                     request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"������ ��� ��������: {request.error}. ������: {request.responseCode}");

                // ��������� ������ (���� ������ �� ��-�� URL)
                if (request.responseCode != 400)
                {
                    Debug.LogWarning("��������� ������� ��������...");
                    StartCoroutine(SendHoneyToServer());
                }
            }
            else
            {
                Debug.LogError("����������� ������: " + request.error);
            }
        }
    }
}
