using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    private string apiUrl = "https://example.com/api/register"; // URL API ��� �����������
    private string uuid; // ��� �������� ����������� UUID

    void Start()
    {
        StartCoroutine(RegisterTeam("QQUQUQUQUQUUQ"));
    }

    IEnumerator RegisterTeam(string teamName)
    {
        // ���� �������
        string jsonBody = JsonUtility.ToJson(new { team_name = teamName });

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(apiUrl, ""))
        {
            // ��������� ����������
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonBody));
            request.downloadHandler = new DownloadHandlerBuffer();

            // �������� �������
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: " + request.downloadHandler.text);

                // ������� ������
                ResponseData response = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);
                uuid = response.uuid;
                Debug.Log($"UUID: {uuid}");

                // ���������� UUID ��� ������������ �������������
                PlayerPrefs.SetString("uuid", uuid);
                PlayerPrefs.Save();
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }

    // ����� ��� �������� ������
    [System.Serializable]
    private class ResponseData
    {
        public string team_name;
        public string uuid;
    }
}
