using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IsEndGameController : MonoBehaviour
{
    public static IsEndGameController instance { get; private set;}

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void CheckEndingGame()
    {
        if(StaticHolder.count_of_simple_honey>= 2 && StaticHolder.count_of_enegry_honey >= 0)
        {
            
            Invoke(nameof(Set_Ending_Scene), 1);
            
        }
    }

    void Set_Ending_Scene()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene("TheEnd");
    }
}
