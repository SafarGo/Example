using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsController : MonoBehaviour
{
    [SerializeField] GameObject BuildingMenu;
    //[SerializeField] GameObject Gun;
    [SerializeField] ObjectOnGridSpawner Spawner;

    [Header("Объеты для удаления при включении")]
    [SerializeField] List <GameObject> mainInventory;
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
        //Gun.SetActive(!isMenuActive);
        StaticHolder.isTurretActive = !isMenuActive;
        StaticHolder.isCanBuild = isMenuActive;
        for (int i = 0; i < mainInventory.Count; i++) { mainInventory[i].SetActive(!isMenuActive); }

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            BuildingMenuController();
        }
    }
}
