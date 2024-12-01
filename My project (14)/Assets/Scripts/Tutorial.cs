using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public bool IsTutor;
    public ReservoirController controller;
    public GameObject Lapa;
    public GameObject LineTutor;
    public GameObject[] TutorPanels;
    public ObjectOnGridSpawner objectSpawner;
    public GameObject EndTutor;
    public int ID_Object;

    private void Update()
    {
        if (Lapa == null)
        {
        }

        if (!IsTutor || objectSpawner == null || controller == null)
        {
            return; // Выход из метода, если не активен или контроллер/объект не заданы
        }

        UpdateTutorialDisplay();
        CheckEndTutorial();
    }

    private void UpdateTutorialDisplay()
    {
        bool isObjectSelected = objectSpawner.SelectedIndex == ID_Object;
        Lapa.SetActive(!isObjectSelected);
        LineTutor.SetActive(isObjectSelected);
    }

    private void CheckEndTutorial()
    {
        if (controller.currentHuneyCount > 0)
        {
            EndTutorial();
        }
    }

    private void EndTutorial()
    {
        TutorPanels[1].SetActive(false);
        EndTutor.SetActive(true);
        Lapa.SetActive(false);
        LineTutor.SetActive(false);
        foreach (GameObject panel in TutorPanels)
        {
            Destroy(panel, 5f);
           
        }
        Destroy(this, 5);
    }
}