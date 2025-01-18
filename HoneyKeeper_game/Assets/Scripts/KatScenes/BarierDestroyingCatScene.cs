using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarierDestroyingCatScene : MonoBehaviour
{
    public static BarierDestroyingCatScene instance;
   [SerializeField] GameObject CameraToKatScene1;
   [SerializeField] GameObject CarCamera;
   [SerializeField] GameObject MainCamera;
    bool borstHide = true;
    float t;
    GameObject Hud;
    void Start()
    {
        Hud = GameObject.Find("PlayerHud--DNC--");
        instance = this;
        CameraToKatScene1.SetActive(false);
    }

    // Update is called once per frame
    public void OnKat_1()
    {
        //StaticController.instance.MuteAllAudioSources();
        CarCamera.SetActive(false);
        MainCamera.SetActive(false);
        Hud.SetActive(false);
        CameraToKatScene1.SetActive(true);
        Debug.Log("on");
    }
    public void OffKat_1()
    {
        //StaticController.instance.RestoreAudioSources();
        Hud.SetActive(true);
        MainCamera.SetActive(true);
        CameraToKatScene1.SetActive(false);
        Destroy(CameraToKatScene1);
        Destroy(gameObject);
        Debug.Log("off");
    }
}
