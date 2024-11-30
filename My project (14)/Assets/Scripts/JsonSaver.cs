using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class JsonSaver : MonoBehaviour
{

    public static DataStorage _data = new DataStorage();//������� ��������� ������ Data Storage
    public static JsonSaver _instance;//������ �� ������
    private string _path;//���� ����������

    private void Awake()
    {
        _path = Path.Combine(Application.persistentDataPath, "save.json");//����������� ����
        _instance = this;

    }

    private void Start()
    {
        //InvokeRepeating(nameof(Save), 5, 5);
    }

    private void OnApplicationQuit() => Save();// ���� ������� �� �����, �������� ������� ����������

    public void Save()
    {
        //����������� ����� �� _data �������� �� ����� StaticHolder 

        _data.AllSpawnedObjects_json = StaticHolder.AllSpawnedObjects;//Chtoto - ������ ���������
        //_data.GameTimeJsom = StaticHolder.GameTime;//Chtoto - ������ ���������
        Debug.Log("hfhfhfhfhfhfhf");

        File.WriteAllText(_path, JsonUtility.ToJson(_data, true));//���������� ����������
    }

    public void Load()
    {

        Debug.Log("1234567890");

        if (File.Exists(_path))//���� ���������� ���� � ������������, �� �� ��������� ��
        {
            _data = JsonUtility.FromJson<DataStorage>(File.ReadAllText(_path));//��������� ����������

            //����������� ����� �� StaticHolder �������� �� _data 
            StaticHolder.AllSpawnedObjects = _data.AllSpawnedObjects_json;//���������� ���������� � ��� ������
            //StaticHolder.GameTime = _data.GameTimeJsom;//���������� ���������� � ��� ������

        }
    }
}

[Serializable]//�����������
public class DataStorage
{
    //������, ������� ����� �������������(����� �������� ���, ��� � StaticHolder ��� ������ ���������� ��������� � ����� �������) 
    public List <GameObject> AllSpawnedObjects_json = new List<GameObject> { };
}
