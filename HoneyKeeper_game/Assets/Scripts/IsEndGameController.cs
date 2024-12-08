using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IsEndGameController : MonoBehaviour
{
    public static IsEndGameController instance { get; private set;}
    bool isEnd = false;
    [SerializeField] GameObject poraText;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Update()
    {
        CheckEndingGame();
    }
    public void CheckEndingGame()
    {
        if(StaticHolder.count_of_simple_honey >= 40 && StaticHolder.count_of_enegry_honey >= 25)
        {
            
            Invoke(nameof(Set_Ending_Scene), 1);
                poraText.SetActive(true);

        }
        else
        {
            poraText.SetActive(false);
        }
        Debug.Log(StaticHolder.count_of_enegry_honey);
        Debug.Log(StaticHolder.count_of_simple_honey);
    }

    void Set_Ending_Scene()
    {
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.Confined;
        isEnd = true;

    }

    private void OnTriggerStay(Collider other)
    {
        if(isEnd)
        {
            SceneManager.LoadScene("TheEnd");
        }
    }
}
