using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class Server : MonoBehaviour
{
    [SerializeField] private string serverURL = "https://2025.nti-gamedev.ru/api/games/bb62ba54-dcaa-4097-89b3-903938dffa60/players/bear1/";
    [SerializeField] private string specificUUID = "bb62ba54-dcaa-4097-89b3-903938dffa60";

    [System.Serializable]
    private class DataToSend
    {
        public string uuid;
        public string playerName;
        public string variableName; // Added variable name field
        public int x;
    }

    public IEnumerator SendDataToSever(int x, string playerName, string variableName)
    {
        DataToSend dataToSend = new DataToSend
        {
            uuid = specificUUID,
            playerName = playerName,
            variableName = variableName, // Assign the variable name
            x = x
        };

        string jsonData = JsonUtility.ToJson(dataToSend);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(serverURL, jsonData))
        {
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                string errorMessage = www.error;
                if (www.isNetworkError)
                    errorMessage += " (Network Error)";
                else if (www.isHttpError)
                    errorMessage += " (" + www.responseCode + ")";
                Debug.LogError("Error Sending Data: " + errorMessage);
            }
            else
            {
                Debug.Log("Data Sent Successfully! UUID: " + specificUUID + ", Player: " + playerName + ", Variable: " + variableName);
                Debug.Log("Server Response: " + www.downloadHandler.text);
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(SendDataToSever(10, "bear1", "honey"));
        }
    }
}
