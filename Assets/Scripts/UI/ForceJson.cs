using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class ForceJson : MonoBehaviour
{
    [SerializeField] public TMP_InputField tmpro;
    [SerializeField] public TextMeshProUGUI clippyDisplay;
    [SerializeField] public ObjectTimer objectTimer;

	private bool success = false;

    string respuestaLimpia;
    static bool errorNoCargarJson = false;

    public void ForceJsonCreation()
    {
        respuestaLimpia = ExtraccionJSON.ExtraerObjetos(tmpro.text);
        Debug.Log("respuestaLimpia: " + respuestaLimpia);
        ClippyScript();

        if (!errorNoCargarJson)
        {
            GestorHistorialIA.GuardarRespuestaForzada(tmpro.text);
        }
        else
        {
            Debug.LogWarning("ErrorNoCargarJson " + errorNoCargarJson);
        }
		if (success)
		{
			tmpro.SetTextWithoutNotify("");
			objectTimer.IniciarMostrado();
		}

    }



    public void ClippyScript()
    {
		success = false;
        errorNoCargarJson = false;

        string res = "Objects: \n";

		Debug.Log($"respuestaLimpia == null ¿? -> {respuestaLimpia}");
        if (respuestaLimpia == null)
        {
            if (VariablesLoader.Data.language.Equals("english"))
            {
                clippyDisplay.SetText("Seems like there's a problem with the response you are trying to push. \n The response wasn't correctly formatted or did not contain the Json. Please make sure the response is complete");
            }else{
                clippyDisplay.text = "Parece que hubo un problema al intentar cargar la respuesta de la IA que estás intentado meter. \n La respuesta no tiene el formato correcto o no contiene el Json. Por favor asegurese de que la respuesta de la IA está completa";
            }
            errorNoCargarJson = true;
        }
        else
        {
            Escena3D escena = JsonUtility.FromJson<Escena3D>(respuestaLimpia);

            foreach (Objeto3D objeto in escena.objects)
            {
                if (GeneradorDeEscena3D.mainObjects.Contains(objeto.type) || GeneradorDeEscena3D.customObjects.Contains(objeto.type))
                {
                    res += " - " + objeto.type + "  x: " + objeto.position.x + "  y: " + objeto.position.y + "  z: " + objeto.position.z  + "\n";
                }
                else
                {
                    if (VariablesLoader.Data.language.Equals("english"))
                    {
                        res = "WARNING, you are trying to load an object that does not exist: " + objeto.type + "\n" + res;
                    }
                    else
                    {
                        res = "¡ERROR!, estás intentado cargar un objeto que no existe: " + objeto.type + "\n" + res;
                    }
                }
            }

            clippyDisplay.text = res;
			success = true;
        }
    }
}
