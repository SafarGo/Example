using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject puddlePrefab; // Префаб для лужи

    private void OnCollisionEnter(Collision collision)
    {
        // Создаем лужу при столкновении с любым объектом
        CreatePuddle(); // Создаем лужу
        Destroy(gameObject); // Уничтожаем снаряд сразу после создания лужи
    }

    private void CreatePuddle()
    {
        if (puddlePrefab != null)
        {
            // Создаем лужу на позиции снаряда
            Instantiate(puddlePrefab, transform.position, Quaternion.identity);
        }
    }
}
