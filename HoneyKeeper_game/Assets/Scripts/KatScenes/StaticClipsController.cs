using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticClipsController : MonoBehaviour
{
    public static StaticClipsController instance {get; private set; }
    AudioSource audioSource;
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (instance == null)
            instance = this;
    }

    public void ActivateClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
        Debug.Log("tobot_soobshil_o_smerty");
    }
}
