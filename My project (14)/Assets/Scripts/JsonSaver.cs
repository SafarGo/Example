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
        Debug.Log(_path);
    }

    private void Start()
    {
        //InvokeRepeating(nameof(Save), 5, 5);
    }

    private void OnApplicationQuit() => Save();// ���� ������� �� �����, �������� ������� ����������

    public void Save()
    {
        //����������� ����� �� _data �������� �� ����� StaticHolder 

        _data.AllSpawnedObjectsID_json = StaticHolder.AllSpawnedObjectsID;//Chtoto - ������ ���������
        _data.AllSpawnedObjectsTranforms_json = StaticHolder.AllSpawnedObjectsTranforms;//Chtoto - ������ ���������
        _data.AllSpawnedObjectsRotations_json = StaticHolder.AllSpawnedObjectsRotations;//Chtoto - ������ ���������
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
            StaticHolder.AllSpawnedObjectsID = _data.AllSpawnedObjectsID_json;//���������� ���������� � ��� ������
            StaticHolder.AllSpawnedObjectsTranforms = _data.AllSpawnedObjectsTranforms_json;//���������� ���������� � ��� ������
            StaticHolder.AllSpawnedObjectsRotations = _data.AllSpawnedObjectsRotations_json;//���������� ���������� � ��� ������
            //StaticHolder.GameTime = _data.GameTimeJsom;//���������� ���������� � ��� ������

        }
    }
}

[Serializable]//�����������
public class DataStorage
{
    //������, ������� ����� �������������(����� �������� ���, ��� � StaticHolder ��� ������ ���������� ��������� � ����� �������) 
    public List <int> AllSpawnedObjectsID_json = new List<int> { };
    public List <Vector3> AllSpawnedObjectsTranforms_json = new List<Vector3> { };
    public List <Quaternion> AllSpawnedObjectsRotations_json = new List<Quaternion> { };
}
