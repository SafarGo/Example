using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Tutorial : MonoBehaviour
{
    public bool IsTutor;
    public ReservoirController controller;
    public GameObject Lapa;
    public GameObject LineTutor;
    public GameObject[] TutorPanels;
    public GameObject ConveyerText;
    public ObjectOnGridSpawner objectSpawner;
    public GameObject EndTutor;
    public int ID_Object;


    private void Start()
    {
        JsonSaver._instance.Load();   
    }
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
        if (controller.currentHuneyCount > 3)
        {
            EndTutorial();
        }
    }

    private void EndTutorial()
    {
        StaticHolder.isFirstGame = false;
        ConveyerText.SetActive(false);
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

    private void DestroyAll()
    {
        foreach (GameObject panel in TutorPanels)
        {
            Destroy(panel);

        }
        Destroy(this);
        ConveyerText.SetActive(false);
        TutorPanels[1].SetActive(false);
        EndTutor.SetActive(false);
        Lapa.SetActive(false);
        LineTutor.SetActive(false);
    }
        

}