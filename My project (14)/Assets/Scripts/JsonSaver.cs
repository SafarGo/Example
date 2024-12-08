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
        _data.isFirstGame_json = StaticHolder.isFirstGame;//Chtoto - ������ ���������
        _data.simple_honey_count = StaticHolder.count_of_simple_honey;//Chtoto - ������ ���������
        _data.energo_honey_count = StaticHolder.count_of_enegry_honey;//Chtoto - ������ ���������
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
            StaticHolder.isFirstGame = _data.isFirstGame_json;//���������� ���������� � ��� ������
            StaticHolder.count_of_simple_honey = _data.simple_honey_count;//���������� ���������� � ��� ������
            StaticHolder.count_of_enegry_honey = _data.energo_honey_count;//���������� ���������� � ��� ������
            //StaticHolder.GameTime = _data.GameTimeJsom;//���������� ���������� � ��� ������

        }
    }

    public void DelateSavings()
    {
        StaticHolder.isFirstGame = true;
        StaticHolder.AllSpawnedObjectsID = new List<int> { }; ;//���������� ���������� � ��� ������
        StaticHolder.AllSpawnedObjectsTranforms = new List<Vector3> { };//���������� ���������� � ��� ������
        StaticHolder.AllSpawnedObjectsRotations = new List<Quaternion> { };//���������� ���������� � ��� ������
        StaticHolder.count_of_simple_honey = 3;
        StaticHolder.count_of_enegry_honey = 3;

        _data.AllSpawnedObjectsID_json = new List<int> { };//Chtoto - ������ ���������
        _data.AllSpawnedObjectsTranforms_json = new List<Vector3> { };//Chtoto - ������ ���������
        _data.AllSpawnedObjectsRotations_json = new List<Quaternion> { };//Chtoto - ������ ���������
        _data.isFirstGame_json = true;//Chtoto - ������ ���������
        _data.simple_honey_count = 3;//Chtoto - ������ ���������
        _data.energo_honey_count = 3;//Chtoto - ������ ���������
        Save();
    }
}

[Serializable]//�����������
public class DataStorage
{
    //������, ������� ����� �������������(����� �������� ���, ��� � StaticHolder ��� ������ ���������� ��������� � ����� �������) 
    public List <int> AllSpawnedObjectsID_json = new List<int> { };
    public List <Vector3> AllSpawnedObjectsTranforms_json = new List<Vector3> { };
    public List <Quaternion> AllSpawnedObjectsRotations_json = new List<Quaternion> { };
    public bool isFirstGame_json = true;
    public int simple_honey_count = 3;
    public int energo_honey_count =3;
}
