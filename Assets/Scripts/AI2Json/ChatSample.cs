using UnityEngine;
using System.Linq;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using System;

public class ChatSample : MonoBehaviour
{
	[SerializeField] private TextAsset systemPromptFile;
	[SerializeField] public TextMeshProUGUI textAuxMenu;
	string systemPrompt = "Empty string";
	public static List<string> AllObjectsList = new List<string>();

	public void StartChatSample()
	{
		GameObject[] prefabs = Resources.LoadAll<GameObject>("Objetos3D");
		string nombres = string.Join(", ", prefabs.Select(p => p.name).ToArray());

		string nombresExternos = string.Join(", ", CustomObjectsLoader.prefabCache.Select(go => go.name));

		AllObjectsList.Clear();
		for (int i = 0; i < prefabs.Length; i++) { AllObjectsList.Add(prefabs[i].name); }
		for (int i = 0; i < CustomObjectsLoader.prefabCache.Count; i++) { AllObjectsList.Add(CustomObjectsLoader.prefabCache[i].name); }

		systemPrompt = systemPromptFile.text +
			"\n Here are all the objects you are allowed to use; you cannot invent new ones. It is imperative that the \"type\" in the JSON is one of the following: "
			+ nombres + ", " + nombresExternos;

		textAuxMenu.text = "items: " + nombres + ", " + nombresExternos;

		Debug.Log("systemPrompt: " + systemPrompt);
	}

	public void CopiarTexto()
	{
		GUIUtility.systemCopyBuffer = systemPrompt + "\n \n \n \n Escribe aquí tu temática a enseñar: ";
		Debug.Log("systemPrompt copiado al portapapeles");
	}

}