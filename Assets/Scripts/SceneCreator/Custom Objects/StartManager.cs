using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public ChatSample chatSample; // Asignar en el Inspector
    public GeneradorDeEscena3D generador;
    public FlyCamera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    /// <summary>
    /// Primero: PermanentData -> VariablesLoader
    /// Segundo: -> FolderPicker -> PermanentData
    /// Tercero: FolderPicker + folderP.DisplayText-> CustomObjectsLoader (Coroutine)
    /// Cuarto: CustomObjectsLoader.StartCallFromChatSample(this);
    /// Quinto.1: ChatSample
    /// Quinto.2: Generador de escena 3d
    /// </summary>
    private bool buttonPressedFolderPick = false;
    [SerializeField] public Canvas tutorialScreen;
    [SerializeField] public GameObject tutorialScreen1;
    [SerializeField] public GameObject LoadingScreen;
    [SerializeField] public GameObject Crosshair;
    [SerializeField] public GameObject Tutorial;
    [SerializeField] public GameObject CanvasChat;
    [SerializeField] public GameObject ChatSample;
    [SerializeField] public GameObject AuxiliaryMenu;
    [SerializeField] public GameObject ClickMenu;
    [SerializeField] public GameObject RoomList;
    [SerializeField] public LanguageButton languageButton;
    [SerializeField] public ClickMenu clickMenu;
    [SerializeField] public RoomSorroundings roomSorroundings;
    [SerializeField] public TutorialPopUp tutorialPopUp ;

    void Start()
    {
        //Poner en activo las pantallas necesarias, y desactivadas.
        cam.transform.root.gameObject.SetActive(false);
        tutorialScreen1.SetActive(true);
        LoadingScreen.SetActive(true);
        Crosshair.SetActive(true);
        CanvasChat.SetActive(true);
        ChatSample.SetActive(true);
        AuxiliaryMenu.SetActive(false);
        ClickMenu.SetActive(false);
        Tutorial.SetActive(true);
        RoomList.SetActive(true);

        VariablesLoader.LoadOrCreate();
        cam.transform.root.gameObject.SetActive(true);
        
        buttonPressedFolderPick = VariablesLoader.Data.folderPicked;
        if (!buttonPressedFolderPick)
            StartCoroutine(DoTutorial());
        else
        {
            tutorialScreen.gameObject.SetActive(false);
            Step2();
        }
        /* Esto va desp de que se cree VariablesLoader */
        if (!VariablesLoader.Data.tutorialPlayed)
            Tutorial.SetActive(true);
        else Tutorial.SetActive(false);

        /* Esto va desp de que se cree VariablesLoader */
        languageButton.StartLanguage();
        tutorialPopUp.StartTutorialPopUp();
        clickMenu.StartClickMenu();
        roomSorroundings.Start();
        Debug.Log("RoomSorroundings must have started by now");
        roomSorroundings.setSorroundingRoom(VariablesLoader.Data.roomNumber);
        
    }

    private IEnumerator DoTutorial()
    {
        // Bucle que espera a que la variable cambie
        while (!buttonPressedFolderPick)
        {
            yield return null; // espera un frame
        }
        // Continúa después de la pulsación
        Debug.Log("Botón pulsado, continúa el tutorial");
        tutorialScreen.gameObject.SetActive(false);
        Step2();
    }

    public void OnFolderPickButtonPressed()
    {
        buttonPressedFolderPick = true;
    }

    void Step2()
    {
        FolderPicker.InitializeForGettingFolderPath();
        FolderPicker.DisplayPathOnText();
        cam.StartFlyCamera();


        StartCoroutine(LoadThenStartSystems());
    }

    private IEnumerator LoadThenStartSystems()
    {
        CustomObjectsLoader.loadingBar = GameObject.Find("LoadingBar").GetComponent<Slider>();
        CustomObjectsLoader.loadingText = GameObject.Find("LoadingText").GetComponent<TextMeshProUGUI>();
        yield return StartCoroutine(CustomObjectsLoader.LoadAllCustomObjectsCoroutine());

        // Esperamos explícitamente a que isLoadingComplete se active
        while (!CustomObjectsLoader.isLoadingComplete)
            yield return null;
        Debug.Log("Carga completa confirmada. Iniciando sistemas dependientes..." + string.Join(", ", CustomObjectsLoader.prefabCache));

        yield return StartCoroutine(generador.EsperarCargaYUsar());

        chatSample.StartChatSample();
    }
}
