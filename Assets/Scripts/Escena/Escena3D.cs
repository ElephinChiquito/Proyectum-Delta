using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Escena3D
{
    public List<Objeto3D> objects = new List<Objeto3D>();
    [NonSerialized] public Activity activity;

	// Wrappers solo para leer la parte "activity" del JSON
	[Serializable] class TipoActivity { public string type; }
	[Serializable] class WrapTipo     { public TipoActivity activity; }
	[Serializable] class WrapTest     { public TestActivityData activity; }

	public static Escena3D Cargar(string json)
	{
		// 1. Objetos: se cargan solos. "activity" se ignora porque no es campo serializable
		Escena3D escena = JsonUtility.FromJson<Escena3D>(json);

		// 2. Leer solo el type de la actividad
		string tipo = JsonUtility.FromJson<WrapTipo>(json).activity?.type;
		Debug.Log("type de la actividad: " + tipo);

		// 3. Leer la actividad con su clase concreta y meterla a mano
		switch (tipo)
		{
			case TestActivityData.TYPE:
				TestActivityData aux = JsonUtility.FromJson<WrapTest>(json).activity; 
				escena.activity = aux;
				break;
			
			default:
				if (!string.IsNullOrEmpty(tipo))
					UnityEngine.Debug.LogWarning("Tipo de actividad desconocido: " + tipo);
				break;   // sin "activity" en el JSON, escena.activity queda en null
		}
		return escena;
	}

	public string ToJson()
	{
		// Objetos: "activity" no sale porque es [NonSerialized]
		string json = JsonUtility.ToJson(this, true);
		if (activity == null) return json;

		// A nivel raíz, ToJson usa el tipo real, así que sí incluye "questions"
		string actJson = JsonUtility.ToJson(activity, true);

		int fin = json.LastIndexOf('}');
		return json.Substring(0, fin).TrimEnd() + ",\n\"activity\": " + actJson + "\n}";
	}
}
