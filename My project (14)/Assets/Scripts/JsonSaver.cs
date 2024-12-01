using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class JsonSaver : MonoBehaviour
{

    public static DataStorage _data = new DataStorage();//—оздает экземпл€р класса Data Storage
    public static JsonSaver _instance;//—сылка на скрипт
    private string _path;//путь сохранени€

    private void Awake()
    {
        _path = Path.Combine(Application.persistentDataPath, "save.json");//ѕрисваиваем путь
        _instance = this;
        Debug.Log(_path);
    }

    private void Start()
    {
        //InvokeRepeating(nameof(Save), 5, 5);
    }

    private void OnApplicationQuit() => Save();// ≈сли выходим из сцены, вызываем функцию сохранени€

    public void Save()
    {
        //присваиваем пол€м из _data значени€ из полей StaticHolder 

        _data.AllSpawnedObjectsID_json = StaticHolder.AllSpawnedObjectsID;//Chtoto - просто перменна€
        _data.AllSpawnedObjectsTranforms_json = StaticHolder.AllSpawnedObjectsTranforms;//Chtoto - просто перменна€
        _data.AllSpawnedObjectsRotations_json = StaticHolder.AllSpawnedObjectsRotations;//Chtoto - просто перменна€
        //_data.GameTimeJsom = StaticHolder.GameTime;//Chtoto - просто перменна€
        Debug.Log("hfhfhfhfhfhfhf");

        File.WriteAllText(_path, JsonUtility.ToJson(_data, true));//«аписываем сохранение
    }

    public void Load()
    {

        Debug.Log("1234567890");

        if (File.Exists(_path))//≈сли существует файл с сохранени€ми, то мы загружаем их
        {
            _data = JsonUtility.FromJson<DataStorage>(File.ReadAllText(_path));//«агружаем сохранение

            //присваиваем пол€м из StaticHolder значени€ из _data 
            StaticHolder.AllSpawnedObjectsID = _data.AllSpawnedObjectsID_json;//засовываем сохранение в наш проект
            StaticHolder.AllSpawnedObjectsTranforms = _data.AllSpawnedObjectsTranforms_json;//засовываем сохранение в наш проект
            StaticHolder.AllSpawnedObjectsRotations = _data.AllSpawnedObjectsRotations_json;//засовываем сохранение в наш проект
            //StaticHolder.GameTime = _data.GameTimeJsom;//засовываем сохранение в наш проект

        }
    }
}

[Serializable]//—ериализуем
public class DataStorage
{
    //данные, которые будем сериализовать(лучше называть так, как в StaticHolder или другом глобальном хранилище в вашем проекте) 
    public List <int> AllSpawnedObjectsID_json = new List<int> { };
    public List <Vector3> AllSpawnedObjectsTranforms_json = new List<Vector3> { };
    public List <Quaternion> AllSpawnedObjectsRotations_json = new List<Quaternion> { };
}
