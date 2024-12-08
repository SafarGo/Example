using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsController : MonoBehaviour
{
    [SerializeField] GameObject BuildingMenu;
    [SerializeField] ObjectOnGridSpawner Spawner;
    bool isMenuActive = false;

    private void Start()
    {
        Spawner.enabled = false;
        BuildingMenu.SetActive(false);
    }
    public void BuildingMenuController()
    {
        isMenuActive = !isMenuActive;
        Spawner.enabled = isMenuActive;
        BuildingMenu.SetActive(isMenuActive);

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            BuildingMenuController();
        }
    }
}
