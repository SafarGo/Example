using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSaver : MonoBehaviour
{
    [System.Serializable]
    public class SavedObjectData
    {
        public string objectName;
        public Vector3 position;
        public Quaternion rotation;
    }

    private List<SavedObjectData> savedObjects = new List<SavedObjectData>();

    void Start()
    {
        PlayerPrefs.DeleteAll();
        LoadObjects();
    }

    private void OnApplicationQuit()
    {
        SaveObjects();
        Debug.Log("Objects saved!");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveObjects();
            Debug.Log("Objects saved!");
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SaveObjects();
            Debug.Log("Objects saved!");
        }
    }

    // Сохраняет объекты в список
    void SaveObjects()
    {
        savedObjects.Clear();

        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (var obj in allObjects)
        {
            if (obj.scene.IsValid() && obj.transform.parent == null) // Только корневые объекты
            {
                savedObjects.Add(new SavedObjectData
                {
                    objectName = obj.name,
                    position = obj.transform.position,
                    rotation = obj.transform.rotation
                });
            }
        }

        string json = JsonUtility.ToJson(new Wrapper<SavedObjectData> { items = savedObjects });
        PlayerPrefs.SetString("SavedObjects", json);
        PlayerPrefs.Save();
    }

    // Загружает объекты из сохранения
    void LoadObjects()
    {
        if (!PlayerPrefs.HasKey("SavedObjects"))
        {
            Debug.Log("No saved objects to load.");
            return;
        }

        string json = PlayerPrefs.GetString("SavedObjects");
        Wrapper<SavedObjectData> wrapper = JsonUtility.FromJson<Wrapper<SavedObjectData>>(json);

        foreach (var savedObject in wrapper.items)
        {
            GameObject obj = GameObject.Find(savedObject.objectName);
            if (obj != null)
            {
                obj.transform.position = savedObject.position;
                obj.transform.rotation = savedObject.rotation;
            }
            else
            {
                Debug.LogWarning($"Object {savedObject.objectName} not found in scene.");
            }
        }

        Debug.Log("Objects loaded!");
    }

    // Вспомогательный класс для сериализации списка
    [System.Serializable]
    public class Wrapper<T>
    {
        public List<T> items;
    }
}
