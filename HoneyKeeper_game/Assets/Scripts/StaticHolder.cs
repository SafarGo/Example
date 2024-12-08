using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticHolder
{
    public static bool isFirstGame = true;
    public static int count_of_simple_honey = 3;
    public static int count_of_enegry_honey = 3;
    public static int count_of_cartriges;
    public static int FlowersCount;
    public static int FlowersHP;
    public static int HivesCount;
    public static List<int> AllSpawnedObjectsID = new List<int> { };
    public static List<Vector3> AllSpawnedObjectsTranforms = new List<Vector3> { };
    public static List<Quaternion> AllSpawnedObjectsRotations = new List<Quaternion> { };
}
