using UnityEngine;
using SFB; // StandaloneFileBrowser
using System.IO;
using System.Diagnostics;
using TMPro;
using System;
using System.Collections;
using Unity.VisualScripting;

public class FolderPicker : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI folderPathTextInstance;
    public static TextMeshProUGUI folderPathText;
    public static string[] directoryPath;

	//Para CycleMessageForRepickFolderPath:
	[SerializeField] GameObject messageForRepickFolderPath;


    /// <summary>
    /// Esto te lo pregunta siempre al iniciar, estaria bien guardar en un json la ruta cuando se cargue, despues, otorgar la posibilidad de cambiar la carpeta con 
    /// un boton, pero esto requeriria decirle al usuario que debe cerrar y volver a abrir el proyecto u hacerlo automaticamente con: 
    /// using UnityEngine;
    /* using System.Diagnostics;
    using System.IO;

    public class AppRestarter : MonoBehaviour
    {
        public static void RestartApplication()
        {
    #if UNITY_STANDALONE_WIN
            string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            Process.Start(exePath); // Start a new instance
            Application.Quit();     // Quit the current one
    #endif
        }
    } */
    /// Lo que pasa es que solo es para windows, (aunq a ver, hace mucho se perdio la compatibilidad no?) pero si se quiere, se puede hacer que si no es windows, no se cerraria 
    /// pero ponemos un mensaje en grande de que hay que reiniciar la aplicacion y bloquear todo tipo de accion.
    /// </summary>
    /// <returns></returns>


    void Awake()
    {
        folderPathText = folderPathTextInstance;
		messageForRepickFolderPath.SetActive(false);
    }

    public static void InitializeForGettingFolderPath()
    {
        if (!VariablesLoader.Data.folderPicked)
        {
            directoryPath = StandaloneFileBrowser.OpenFolderPanel("Selecciona la carpeta con sus modelos personales", "", false);
            if (directoryPath.Length > 0)
            {
                VariablesLoader.Data.folderPicked = true;
                VariablesLoader.Data.folderPath = directoryPath[0];
                VariablesLoader.Save();

                UnityEngine.Debug.Log("Ruta seleccionada: " + directoryPath[0] + " Y tambien guardada en: " + VariablesLoader.filePath);
            }
        }
        else
        {
            UnityEngine.Debug.Log("Carpeta ya existente y seleccionada.");
        }


        UnityEngine.Debug.Log("No se seleccionó ninguna carpeta.");
    }

    /// <summary>
    /// Esto deberia reiniciar la ventana en la final build.
    /// </summary>
    public void RePickFolderPath()
    {
        VariablesLoader.Data.folderPicked = false;
        VariablesLoader.Save();

#if UNITY_STANDALONE_WIN
        string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

        Process.Start(exePath); // Start a new instance
        Application.Quit();     // Quit the current one
#endif
    }

	public void CycleMessageForRepickFolderPath()
	{
		messageForRepickFolderPath.SetActive(!messageForRepickFolderPath.activeSelf);
	}

    public static void DisplayPathOnText()
    {
        folderPathText.text = VariablesLoader.Data.folderPath;
    }
}
