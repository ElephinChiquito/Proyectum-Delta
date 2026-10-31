using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Linq;

public class NpcDialog : MonoBehaviour
{
	[TextArea(2, 5)] public string textoDialogo;
	[SerializeField] Canvas canvas;
	[SerializeField] TextMeshProUGUI stepsTMP;
	GameObject camPlayer;
	public TextMeshProUGUI textoUI;
	private string[] segmentos;
	private int step = 0;
	private bool showingDialog = false;

	private void Start()
	{
		// Por defecto, oculta el texto
		textoUI.text = "";
		canvas.GetComponent<RawImage>().gameObject.SetActive(false);
		camPlayer = GameObject.Find("Flying UI Camera");
	}

	public void Update()
    {
        if (showingDialog)
        {
            Vector3 direccion = canvas.gameObject.transform.position - camPlayer.transform.position;
    		canvas.gameObject.transform.rotation = Quaternion.LookRotation(direccion);
        }
    }

	// Puedes añadir más lógica aquí, como mostrar al hacer clic o al acercarse
	public void MostrarDialogo()
	{
		Debug.Log("NPC dice: " + segmentos[step]);
		canvas.GetComponent<RawImage>().gameObject.SetActive(true);
		textoUI.text = segmentos[step];
		stepsTMP.text = (step+1) + "\\" + segmentos.Length;
		CancelInvoke(nameof(OcultarDialogo));

		int numPalabras = segmentos[step].Split(' ').Length;
		float duracion = numPalabras * 0.37f; // 0.3 segundos por palabra
		showingDialog = true;

		Invoke(nameof(OcultarDialogo), duracion);
	}

	void OcultarDialogo()
	{
		if (textoUI != null)
			textoUI.text = "";
		canvas.GetComponent<RawImage>().gameObject.SetActive(false);
		showingDialog = false;
	}

	//Apañar AI Prompt
	void PartirDialogo()
	{
		segmentos = Regex.Matches(textoDialogo, @"[^.]+\.")
                     .Cast<Match>()
                     .Select(m => m.Value.Trim())
                     .ToArray();

    	if(segmentos.Length == 0)
        	segmentos = new string[] { textoDialogo }; // fallback si no hay puntos
	}

	private void OnMouseDown()
	{
		Interact();
	}
	public void Interact()
	{
		// Esto se ejecuta al hacer clic sobre el objeto con collider
		PartirDialogo();

		WindowsVoice.speak(segmentos[step]);
		MostrarDialogo();
		if (segmentos.Length-1 > step) step++;
		else step = 0;

	}
}
