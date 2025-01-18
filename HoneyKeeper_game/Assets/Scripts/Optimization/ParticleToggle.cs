using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleToggle : MonoBehaviour
{
    public float maxDistance = 250; // Максимальное расстояние до Player
    private GameObject player; // Объект Player
    private Transform[] particleObjects; // Массив дочерних объектов с тегом "particle_tag"
    [SerializeField] float step = 1f;

    private void Start()
    {
        if(step == 0)
            step = 1;
        // Ищем объект с тегом "Player"
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player с тегом 'Player' не найден!");
            return;
        }

        // Получаем все дочерние объекты с тегом "particle_tag"
        particleObjects = GetChildrenWithTag(transform, "particle_tag");

        // Запускаем проверку каждые 2 секунды
       StartCoroutine(CheckDistance());
    }

    private void Update()
    {
        // Проверяем, находятся ли дочерние объекты в пределах камеры
        CheckVisibility();
    }

    // Получение всех дочерних объектов с указанным тегом
    private Transform[] GetChildrenWithTag(Transform parent, string tag)
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>();
        List<Transform> taggedChildren = new List<Transform>();

        foreach (Transform child in children)
        {
            if (child.CompareTag(tag))
            {
                taggedChildren.Add(child);
            }
        }

        return taggedChildren.ToArray();
    }

    // Проверка расстояния каждые 2 секунды
    private IEnumerator CheckDistance()
    {
        while (true)
        {
            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);

                if (distance > maxDistance)
                {
                    SetParticlesActive(false);
                }
                else
                {
                    SetParticlesActive(true);
                }
            }

            yield return new WaitForSeconds(step); // Интервал проверки
        }
    }

    // Включение/выключение дочерних объектов с тегом "particle_tag"
    private void SetParticlesActive(bool isActive)
    {
        if (particleObjects != null)
        {
            foreach (Transform particle in particleObjects)
            {
                particle.gameObject.SetActive(isActive);
            }
        }
    }

    // Проверка видимости дочерних объектов в камере
    private void CheckVisibility()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        if (particleObjects != null)
        {
            foreach (Transform particle in particleObjects)
            {
                Renderer renderer = particle.GetComponent<Renderer>();
                if (renderer != null)
                {
                    // Если объект не виден, выключаем его
                    if (!renderer.isVisible)
                    {
                        particle.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
