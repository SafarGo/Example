using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    //private float _maxSoundTrackVolume;
    //private float _minSoundTrackVolume;
    //float PrefsVoluve;
    [SerializeField] private AudioSource soundTrakcAudioSource;
    [SerializeField] private Slider soundtrackVolumeSlider;

    //private float _maxSensivity;
    //private float _minSensivity;
    //float PrefsSensivity;
    [SerializeField] private FirstPersonController firstPersonController;
    [SerializeField] private Slider sensivitySlider;

    private void Start()
    {
        if(PlayerPrefs.GetFloat("PrefsVolume_p") == 0 || PlayerPrefs.GetFloat("PrefsSensivity_p") == 0)
        {
            PlayerPrefs.SetFloat("PrefsVolume_p", 0.104f);
            PlayerPrefs.SetFloat("PrefsSensivity_p", 2);
        }
        soundtrackVolumeSlider.value = PlayerPrefs.GetFloat("PrefsVolume_p");
        sensivitySlider.value = PlayerPrefs.GetFloat("PrefsSensivity_p");
        UpdateSaundtrackVolume();
        UpdateSensivity();
    }

    public void UpdateSaundtrackVolume()
    {
        soundTrakcAudioSource.volume = soundtrackVolumeSlider.value;
        PlayerPrefs.SetFloat("PrefsVolume_p", soundtrackVolumeSlider.value);
        Debug.Log(PlayerPrefs.GetFloat("PrefsVolume_p") + "++++++++++++++");
    }

    public void UpdateSensivity()
    {
        firstPersonController.mouseSensitivity = sensivitySlider.value;
        PlayerPrefs.SetFloat("PrefsSensivity_p", sensivitySlider.value);
    }

}
