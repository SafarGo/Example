using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClumbsManager
{
    public static float FlowerCounts = 0f;

    // ����� ��� ���������� ����� freshment ���� ��������
    public static void UpdateCounts(float newValue)
    {
        FlowerCounts = newValue;
        Debug.Log($"����������� ����� freshment: {FlowerCounts}");
    }
}
