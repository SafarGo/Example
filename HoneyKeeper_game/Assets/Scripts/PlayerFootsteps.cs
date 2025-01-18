using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerFootsteps : MonoBehaviour
{
    [System.Serializable]
    public class SurfaceSoundSettings
    {
        public LayerMask surfaceLayer;         // Слой поверхности
        public AudioClip[] footstepSounds;     // Звуки шагов
        public AudioClip jumpSound;            // Звук прыжка
        public float stepRate = 0.5f;          // Время между шагами
    }

    public List<SurfaceSoundSettings> surfaceSettings = new List<SurfaceSoundSettings>();
    public float raycastDistance = 1.5f;      // Дистанция рейкаста для определения поверхности

    public Color raycastHitColor = Color.green; // Цвет рейкаста при успешном попадании
    public Color raycastMissColor = Color.red;  // Цвет рейкаста, если поверхность не найдена

    private AudioSource audioSource;
    private float stepTimer;
    private int currentSurfaceLayer = -1; // Слой текущей поверхности (-1, если не найдено)
    private Vector3 lastPosition;
    private bool isGrounded = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        stepTimer = 0f;
        lastPosition = transform.position;
    }

    private void Update()
    {
        DetectGround();

        if (isGrounded)
        {
            HandleFootsteps();
        }
        else
        {
            stepTimer = 0f; // Сбрасываем таймер шагов, если персонаж в воздухе
        }
    }

    private void DetectGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            isGrounded = true;
            currentSurfaceLayer = hit.collider.gameObject.layer;
        }
        else
        {
            isGrounded = false;
            currentSurfaceLayer = -1;
        }
    }

    private void HandleFootsteps()
    {
        float speed = (transform.position - lastPosition).magnitude / Time.deltaTime;

        if (speed > 0.1f) // Если персонаж двигается
        {
            stepTimer += Time.deltaTime;
            SurfaceSoundSettings currentSurface = GetSurfaceSettings(currentSurfaceLayer);

            if (currentSurface != null && stepTimer >= currentSurface.stepRate)
            {
                PlayFootstepSound(currentSurface);
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f; // Сбрасываем таймер шагов, если персонаж стоит
        }

        lastPosition = transform.position;
    }

    private void PlayFootstepSound(SurfaceSoundSettings surface)
    {
        if (surface.footstepSounds.Length > 0)
        {
            AudioClip clip = surface.footstepSounds[Random.Range(0, surface.footstepSounds.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    private SurfaceSoundSettings GetSurfaceSettings(int layer)
    {
        foreach (SurfaceSoundSettings settings in surfaceSettings)
        {
            if (((1 << layer) & settings.surfaceLayer) != 0)
            {
                return settings;
            }
        }
        return null;
    }

    public void PlayJumpSound()
    {
        SurfaceSoundSettings currentSurface = GetSurfaceSettings(currentSurfaceLayer);
        if (currentSurface != null && currentSurface.jumpSound != null)
        {
            audioSource.PlayOneShot(currentSurface.jumpSound);
        }
    }

    // Рисование рейкаста в редакторе
    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down * raycastDistance;

        RaycastHit hit;
        if (Physics.Raycast(origin, Vector3.down, out hit, raycastDistance))
        {
            Gizmos.color = raycastHitColor;
            Gizmos.DrawLine(origin, hit.point);
            Gizmos.DrawSphere(hit.point, 0.1f);
        }
        else
        {
            Gizmos.color = raycastMissColor;
            Gizmos.DrawLine(origin, origin + direction);
        }
    }

    // Рисование рейкаста в режиме редактирования (включая Play Mode и Editor Mode)
    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down * raycastDistance;

        RaycastHit hit;
        if (Physics.Raycast(origin, Vector3.down, out hit, raycastDistance))
        {
            Gizmos.color = raycastHitColor;
            Gizmos.DrawLine(origin, hit.point);
            Gizmos.DrawSphere(hit.point, 0.1f);
        }
        else
        {
            Gizmos.color = raycastMissColor;
            Gizmos.DrawLine(origin, origin + direction);
        }
    }
}
