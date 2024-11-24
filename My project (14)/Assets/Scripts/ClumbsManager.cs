using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClumbsManager
{
    public static float FlowerCounts = 0f;

    // Метод для обновления суммы freshment всех объектов
    public static void UpdateCounts(float newValue)
    {
        FlowerCounts = newValue;
        Debug.Log($"Обновленная сумма freshment: {FlowerCounts}");
    }
}
