using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticHolder
{
    public static bool isCanFire;
    public static bool isCanOpenUI = true;
    public static bool isTurretActive = true;
    public static bool isFirstGame = true;
    public static int count_of_simple_honey = 3;
    public static int count_of_enegry_honey = 3;
    public static int count_of_cartriges;
    public static int FlowersCount;
    public static int FlowersHP;
    public static int HivesCount;
    public static bool OnDrive;
    public static List<int> AllSpawnedObjectsID = new List<int> { };                     //только для сохранений
    public static List<Vector3> AllSpawnedObjectsTranforms = new List<Vector3> { };      //только для сохранений
    public static List<Quaternion> AllSpawnedObjectsRotations = new List<Quaternion> { };//только для сохранений
    public static List<ObjectPlacer> ObstaclesToAttack { get; private set; } = new List<ObjectPlacer>();
    public static void AddObstacle(ObjectPlacer objectPlacer)
    {
        ObstaclesToAttack.Add(objectPlacer);
    }
}
