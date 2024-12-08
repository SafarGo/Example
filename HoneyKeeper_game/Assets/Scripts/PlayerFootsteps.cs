using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public AudioClip[] footstepSounds;
    public float stepInterval = 0.5f;
    public float shiftMultiplier = 0.5f;
    public float volume = 0.5f;
    public float pitchMin = 0.9f;
    public float pitchMax = 1.1f;

    private AudioSource audioSource;
    private float stepTimer;
    private int lastSoundIndex = -1;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.volume = volume;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            stepTimer -= Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 1f / shiftMultiplier : 1f);

            if (stepTimer <= 0f && footstepSounds.Length > 1)
            {
                PlayRandomFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void PlayRandomFootstep()
    {
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, footstepSounds.Length);
        } while (randomIndex == lastSoundIndex);

        lastSoundIndex = randomIndex;

        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.PlayOneShot(footstepSounds[randomIndex], volume);
    }
}
