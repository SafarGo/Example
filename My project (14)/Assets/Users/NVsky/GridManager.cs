using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 10; // Количество ячеек по ширине
    public int gridHeight = 10; // Количество ячеек по высоте
    public float cellSize = 1f; // Размер одной ячейки
    public Color gridColor = Color.gray; // Цвет линии сетки

    void OnDrawGizmos()
    {
        Gizmos.color = gridColor;

        // Рисуем вертикальные линии
        for (int x = 0; x <= gridWidth; x++)
        {
            Vector3 start = new Vector3(x * cellSize, 0, 0);
            Vector3 end = new Vector3(x * cellSize, 0, gridHeight * cellSize);
            Gizmos.DrawLine(start, end);
        }

        // Рисуем горизонтальные линии
        for (int z = 0; z <= gridHeight; z++)
        {
            Vector3 start = new Vector3(0, 0, z * cellSize);
            Vector3 end = new Vector3(gridWidth * cellSize, 0, z * cellSize);
            Gizmos.DrawLine(start, end);
        }
    }

    public Vector3 GetClosestGridPoint(Vector3 position)
    {
        float x = Mathf.Round(position.x / cellSize) * cellSize;
        float z = Mathf.Round(position.z / cellSize) * cellSize;
        return new Vector3(x, position.y, z);
    }
}
