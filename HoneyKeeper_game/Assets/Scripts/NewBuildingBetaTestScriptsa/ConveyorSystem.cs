using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorSystem : MonoBehaviour
{
    [Header("Conveyor Settings")]
    public GameObject conveyorSegmentPrefab;   // Префаб сегмента конвейера
    public float placementInterval = 0.5f;     // Интервал между сегментами
    public string obstacleTag = "obstacle_tag"; // Тег для препятствий
    public float maxBuildDistance = 10f;       // Максимальная дистанция строительства от игрока
    public float snapRadius = 1.0f;            // Радиус для привязки к ближайшему catchPoint

    [Header("Raycast Settings")]
    public LayerMask groundLayer;              // Слои для рейкаста

    [Header("Preview Colors")]
    public Color validColor = Color.green;     // Цвет для валидного размещения
    public Color invalidColor = Color.red;     // Цвет для препятствий

    private Vector3? firstPoint = null;        // Первая выбранная точка
    private Vector3? secondPoint = null;       // Вторая выбранная точка
    private bool isPlacing = false;            // Находится ли игрок в процессе выбора точек
    private List<GameObject> previewSegments = new List<GameObject>(); // Хранение объектов превью
    private bool isPathClear = true;
    [HideInInspector] public bool isCanbuildConveyer;// Указывает, есть ли пересечения

    void Update()
    {
        if(isCanbuildConveyer && StaticHolder.isCanBuild)
        {


            HandleInput();  // Обработка ввода
            UpdatePreview(); // Обновление визуального превью

            if (Input.GetKey(KeyCode.R))
            {
                foreach (var segment in previewSegments)
                {
                    segment.transform.GetChild(0).gameObject.transform.Rotate(0, 180, 0);
                }
            }
        }
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0)) // ЛКМ для выбора точек
        {

            Vector3? hitPoint = GetMousePositionOnGround();
            if (hitPoint.HasValue)
            {
                if (!firstPoint.HasValue)
                {
                    if (Vector3.Distance(transform.position, hitPoint.Value) > maxBuildDistance)
                    {
                        Debug.LogWarning("First point is too far away.");
                        return;
                    }

                    firstPoint = SnapToCatchPoint(hitPoint.Value);
                    isPlacing = true;
                    Debug.Log("First point set at: " + firstPoint.Value);
                }
                else if (!secondPoint.HasValue)
                {
                    if (Vector3.Distance(transform.position, hitPoint.Value) > maxBuildDistance)
                    {
                        Debug.LogWarning("Second point is too far away.");
                        return;
                    }

                    secondPoint = SnapToGrid(hitPoint.Value);
                    isPlacing = false;
                    Debug.Log("Second point set at: " + secondPoint.Value);

                    if (isPathClear)
                    {
                        StartCoroutine(BuildConveyors(firstPoint.Value, secondPoint.Value));
                    }
                    else
                    {
                        Debug.LogWarning("Cannot place conveyors: path intersects with obstacles.");
                        ClearPreview();
                    }

                    firstPoint = secondPoint = null;
                }
            }
        }
    }

    Vector3? GetMousePositionOnGround()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            return hit.point;
        }
        return null;
    }

    Vector3 SnapToCatchPoint(Vector3 position)
    {
        GameObject closestCatchPoint = null;
        float closestDistance = snapRadius;

        foreach (var catchPoint in GameObject.FindGameObjectsWithTag("catchPoint"))
        {
            float distance = Vector3.Distance(position, catchPoint.transform.position);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                closestCatchPoint = catchPoint;
            }
        }

        return closestCatchPoint ? closestCatchPoint.transform.position : SnapToGrid(position);
    }

    IEnumerator BuildConveyors(Vector3 start, Vector3 end)
    {
        Vector3 direction = GetDirection(start, end);
        int segmentCount = Mathf.CeilToInt(Vector3.Distance(start, end) / placementInterval);

        for (int i = 0; i <= segmentCount; i++)
        {
            Vector3 position = start + direction * (i * placementInterval);
            Quaternion rotation = Quaternion.LookRotation(direction);

            // Выравниваем первый и последний сегменты по горизонтали
            if (i == 0 || i == segmentCount)
            {
                rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
            }

            GameObject segment = Instantiate(conveyorSegmentPrefab, position, rotation);

            if (i != segmentCount)
            {
                Destroy(segment.transform.Find("catchPoint")?.gameObject);
            }
        }

        ClearPreview();
        yield return null;
    }


    Vector3 SnapToGrid(Vector3 position)
    {
        return new Vector3(Mathf.Round(position.x), position.y, Mathf.Round(position.z));
    }

    Vector3 GetDirection(Vector3 start, Vector3 end)
    {
        Vector3 direction = (end - start).normalized;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
        {
            return direction.x > 0 ? Vector3.right : Vector3.left;
        }
        else
        {
            return direction.z > 0 ? Vector3.forward : Vector3.back;
        }
    }

    void UpdatePreview()
    {
        if (isPlacing && firstPoint.HasValue)
        {
            Vector3? currentMousePosition = GetMousePositionOnGround();
            if (currentMousePosition.HasValue)
            {
                Vector3 start = firstPoint.Value;
                Vector3 end = SnapToGrid(currentMousePosition.Value);
                Vector3 direction = GetDirection(start, end);
                int segmentCount = Mathf.CeilToInt(Vector3.Distance(start, end) / placementInterval);

                // Увеличение или уменьшение количества сегментов превью
                AdjustPreviewSegments(segmentCount);

                isPathClear = true;
                for (int i = 0; i <= segmentCount; i++)
                {
                    Vector3 position = start + direction * (i * placementInterval);
                    Quaternion rotation = Quaternion.LookRotation(direction);

                    // Выравниваем первый и последний сегменты по горизонтали
                    if (i == 0 || i == segmentCount)
                    {
                        rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
                    }

                    bool isValid = !CheckCollision(position);
                    isPathClear &= isValid;

                    GameObject segment = previewSegments[i];
                    segment.transform.position = position;
                    segment.transform.rotation = rotation;

                    Renderer[] renderers = segment.GetComponentsInChildren<Renderer>();
                    foreach (Renderer renderer in renderers)
                    {
                        renderer.material.color = isValid ? validColor : invalidColor;
                    }
                }
            }
        }
    }


    void AdjustPreviewSegments(int segmentCount)
    {
        while (previewSegments.Count < segmentCount + 1)
        {
            GameObject newSegment = Instantiate(conveyorSegmentPrefab);
            previewSegments.Add(newSegment);
        }

        while (previewSegments.Count > segmentCount + 1)
        {
            GameObject segmentToRemove = previewSegments[previewSegments.Count - 1];
            Destroy(segmentToRemove);
            previewSegments.RemoveAt(previewSegments.Count - 1);
        }
    }

    bool CheckCollision(Vector3 position)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, 0.1f);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag(obstacleTag))
            {
                return true;
            }
        }
        return false;
    }

    public void ClearPreview()
    {
        foreach (var segment in previewSegments)
        {
            Destroy(segment);
        }
        previewSegments.Clear();
    }

    void OnDrawGizmos()
    {
        if (firstPoint.HasValue)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(firstPoint.Value, 0.1f);
        }

        if (secondPoint.HasValue)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(secondPoint.Value, 0.1f);
        }
    }
    

    public void ClearPoints()
    {
        firstPoint = null; secondPoint = null;
    }
}
