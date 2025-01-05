using UnityEditor;
using UnityEngine;

//[ExecuteAlways]
public class RockGenerator : MonoBehaviour
{
    [Header("Настройки деформации")]
    public float deformationIntensity = 0.5f; // Максимальная величина деформации
    public int noiseIterations = 3;          // Количество итераций шума для добавления реалистичности
    public float noiseScale = 0.3f;          // Масштаб шума

    [Header("Размер камня")]
    public Vector3 minScale = new Vector3(0.8f, 0.8f, 0.8f); // Минимальный размер камня
    public Vector3 maxScale = new Vector3(1.5f, 1.5f, 1.5f); // Максимальный размер камня

    public bool generateOnStart = true; // Деформация при старте игры


    public void GenerateRock()
    {
        // Получение MeshFilter
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.mesh == null)
        {
            Debug.LogError("У объекта отсутствует MeshFilter или Mesh.");
            return;
        }

        // Копируем оригинальный меш
        Mesh originalMesh = meshFilter.mesh;
        Mesh deformedMesh = Instantiate(originalMesh);

        Vector3[] vertices = deformedMesh.vertices;

        // Случайная деформация вершин
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];

            // Генерация случайного смещения
            for (int j = 0; j < noiseIterations; j++)
            {
                Vector3 noise = new Vector3(
                    Mathf.PerlinNoise(vertex.x * noiseScale + j, vertex.y * noiseScale + j) - 0.5f,
                    Mathf.PerlinNoise(vertex.y * noiseScale + j, vertex.z * noiseScale + j) - 0.5f,
                    Mathf.PerlinNoise(vertex.z * noiseScale + j, vertex.x * noiseScale + j) - 0.5f
                );

                noise *= deformationIntensity / noiseIterations;
                vertex += noise;
            }

            vertices[i] = vertex;
        }

        // Обновление меша
        deformedMesh.vertices = vertices;
        deformedMesh.RecalculateNormals();
        deformedMesh.RecalculateBounds();

        meshFilter.mesh = deformedMesh;

        // Обновление MeshCollider (если есть)
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = null;
            meshCollider.sharedMesh = deformedMesh;
        }

        // Случайный масштаб камня
        transform.localScale = new Vector3(
            Random.Range(minScale.x, maxScale.x),
            Random.Range(minScale.y, maxScale.y),
            Random.Range(minScale.z, maxScale.z)
        );
    }


    //private void Update()
    //{
    //    Debug.LogError("NullReferenceException");
    //}

}
#if UNITY_EDITOR
[CustomEditor(typeof(RockGenerator), false)]
public sealed class RockGeneratorEditor : Editor
{
    private RockGenerator _rockGenerator;

    private void OnEnable()
    {
        _rockGenerator = target as RockGenerator;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Block", GUILayout.Height(15)))
        {
            Undo.RecordObject(_rockGenerator.gameObject, "Object change mesh");
            _rockGenerator.GenerateRock();
        }
    }
}
#endif
