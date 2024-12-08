using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ObstaclesDestroyer : MonoBehaviour
{
    [SerializeField] private LayerMask selectableLayer; // Слой для рейкаста
    [SerializeField] private float holdTime = 5f;       // Время удержания для удаления
    private Slider slider;
    private float holdTimer;                            // Таймер удержания
    private GameObject targetObject;                   // Целевой объект рейкаста


    private void Start()
    {
        slider = GameObject.Find("Destroyer").GetComponent<Slider>();
        slider.gameObject.SetActive(false);
    }
    void Update()
    {
        // Рейкаст при нажатии ЛКМ
        if (Input.GetMouseButtonDown(0))
            StartRaycast();

        // Удержание ЛКМ
        if (Input.GetMouseButton(0) && targetObject)
            CheckHold();
        else
            slider.gameObject.SetActive(false);

        // Сброс, если отпущена ЛКМ
        if (Input.GetMouseButtonUp(0))
            ResetTarget();

        if(!targetObject)
        {
            slider.gameObject.SetActive(false);
        }
        else
        {
            slider.gameObject.SetActive(true);
        }
    }

    private void StartRaycast()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
            out RaycastHit hit, Mathf.Infinity, selectableLayer)
            && hit.collider.CompareTag("cantDestroyObstacle"))
        {
            //slider.gameObject.SetActive(true);
            targetObject = hit.collider.gameObject;
            holdTimer = 0f; // Сброс таймера
        }
        else
        {
            targetObject = null;
            slider.gameObject.SetActive(false);
        }
    }

    private void CheckHold()
    {
        holdTimer += Time.deltaTime;
        slider.value = holdTimer * 0.5f;
        if (holdTimer >= holdTime)
        {
            //targetObject.GetComponent<StoneSaving>().Destroying();
            Destroy(targetObject); // Удаление объекта
            ResetTarget();
            slider.gameObject.SetActive(false);
            slider.value = 0;// Сброс
        }
    }

    private void ResetTarget()
    {
        targetObject = null;
        holdTimer = 0f;
    }
}
