using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IsEndGameController : MonoBehaviour
{
    public static IsEndGameController instance { get; private set;}
    [SerializeField] GameObject CanEnd;
    bool isCanEnd = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        //InvokeRepeating(nameof(UpdateInfo), 5,5);
    }

    void Set_Ending_Scene()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene("TheEnd");
    }

    private void FixedUpdate()
    {
        if (StaticHolder.count_of_simple_honey >= 40 && StaticHolder.count_of_enegry_honey >= 25)
        {
            CanEnd.SetActive(true);
            isCanEnd = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(isCanEnd == true && other.CompareTag("Player"))
        {
            Debug.Log("Отлет");
            Debug.Log(isCanEnd);
            Invoke(nameof(Set_Ending_Scene), 2);
        }
    }
}
