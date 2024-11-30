using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class JsonSaver : MonoBehaviour
{

    public static DataStorage _data = new DataStorage();//Создает экземпляр класса Data Storage
    public static JsonSaver _instance;//Ссылка на скрипт
    private string _path;//путь сохранения

    private void Awake()
    {
        _path = Path.Combine(Application.persistentDataPath, "save.json");//Присваиваем путь
        _instance = this;

    }

    private void Start()
    {
        //InvokeRepeating(nameof(Save), 5, 5);
    }

    private void OnApplicationQuit() => Save();// Если выходим из сцены, вызываем функцию сохранения

    public void Save()
    {
        //присваиваем полям из _data значения из полей StaticHolder 

        _data.AllSpawnedObjects_json = StaticHolder.AllSpawnedObjects;//Chtoto - просто перменная
        //_data.GameTimeJsom = StaticHolder.GameTime;//Chtoto - просто перменная
        Debug.Log("hfhfhfhfhfhfhf");

        File.WriteAllText(_path, JsonUtility.ToJson(_data, true));//Записываем сохранение
    }

    public void Load()
    {

        Debug.Log("1234567890");

        if (File.Exists(_path))//Если существует файл с сохранениями, то мы загружаем их
        {
            _data = JsonUtility.FromJson<DataStorage>(File.ReadAllText(_path));//Загружаем сохранение

            //присваиваем полям из StaticHolder значения из _data 
            StaticHolder.AllSpawnedObjects = _data.AllSpawnedObjects_json;//засовываем сохранение в наш проект
            //StaticHolder.GameTime = _data.GameTimeJsom;//засовываем сохранение в наш проект

        }
    }
}

[Serializable]//Сериализуем
public class DataStorage
{
    //данные, которые будем сериализовать(лучше называть так, как в StaticHolder или другом глобальном хранилище в вашем проекте) 
    public List <GameObject> AllSpawnedObjects_json = new List<GameObject> { };
}
