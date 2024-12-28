using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    private void OnEnable()
    {
        // Уничтожаем объект через 5 секунд
        Invoke(nameof(DestroySelf), 5f);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}