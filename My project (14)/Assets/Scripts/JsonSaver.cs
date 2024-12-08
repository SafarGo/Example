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
        Debug.Log(_path);
    }

    private void Start()
    {
        //InvokeRepeating(nameof(Save), 5, 5);
    }

    private void OnApplicationQuit() => Save();// Если выходим из сцены, вызываем функцию сохранения

    public void Save()
    {
        //присваиваем полям из _data значения из полей StaticHolder 

        _data.AllSpawnedObjectsID_json = StaticHolder.AllSpawnedObjectsID;//Chtoto - просто перменная
        _data.AllSpawnedObjectsTranforms_json = StaticHolder.AllSpawnedObjectsTranforms;//Chtoto - просто перменная
        _data.AllSpawnedObjectsRotations_json = StaticHolder.AllSpawnedObjectsRotations;//Chtoto - просто перменная
        _data.isFirstGame_json = StaticHolder.isFirstGame;//Chtoto - просто перменная
        _data.simple_honey_count = StaticHolder.count_of_simple_honey;//Chtoto - просто перменная
        _data.energo_honey_count = StaticHolder.count_of_enegry_honey;//Chtoto - просто перменная
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
            StaticHolder.AllSpawnedObjectsID = _data.AllSpawnedObjectsID_json;//засовываем сохранение в наш проект
            StaticHolder.AllSpawnedObjectsTranforms = _data.AllSpawnedObjectsTranforms_json;//засовываем сохранение в наш проект
            StaticHolder.AllSpawnedObjectsRotations = _data.AllSpawnedObjectsRotations_json;//засовываем сохранение в наш проект
            StaticHolder.isFirstGame = _data.isFirstGame_json;//засовываем сохранение в наш проект
            StaticHolder.count_of_simple_honey = _data.simple_honey_count;//засовываем сохранение в наш проект
            StaticHolder.count_of_enegry_honey = _data.energo_honey_count;//засовываем сохранение в наш проект
            //StaticHolder.GameTime = _data.GameTimeJsom;//засовываем сохранение в наш проект

        }
    }

    public void DelateSavings()
    {
        StaticHolder.isFirstGame = true;
        StaticHolder.AllSpawnedObjectsID = new List<int> { }; ;//засовываем сохранение в наш проект
        StaticHolder.AllSpawnedObjectsTranforms = new List<Vector3> { };//засовываем сохранение в наш проект
        StaticHolder.AllSpawnedObjectsRotations = new List<Quaternion> { };//засовываем сохранение в наш проект
        StaticHolder.count_of_simple_honey = 3;
        StaticHolder.count_of_enegry_honey = 3;

        _data.AllSpawnedObjectsID_json = new List<int> { };//Chtoto - просто перменная
        _data.AllSpawnedObjectsTranforms_json = new List<Vector3> { };//Chtoto - просто перменная
        _data.AllSpawnedObjectsRotations_json = new List<Quaternion> { };//Chtoto - просто перменная
        _data.isFirstGame_json = true;//Chtoto - просто перменная
        _data.simple_honey_count = 3;//Chtoto - просто перменная
        _data.energo_honey_count = 3;//Chtoto - просто перменная
        Save();
    }
}

[Serializable]//Сериализуем
public class DataStorage
{
    //данные, которые будем сериализовать(лучше называть так, как в StaticHolder или другом глобальном хранилище в вашем проекте) 
    public List <int> AllSpawnedObjectsID_json = new List<int> { };
    public List <Vector3> AllSpawnedObjectsTranforms_json = new List<Vector3> { };
    public List <Quaternion> AllSpawnedObjectsRotations_json = new List<Quaternion> { };
    public bool isFirstGame_json = true;
    public int simple_honey_count = 3;
    public int energo_honey_count =3;
}
