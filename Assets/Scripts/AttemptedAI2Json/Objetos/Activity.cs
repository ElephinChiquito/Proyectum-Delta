using System;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;


[Serializable]
public class Activity: Objeto3D
{
    public Pregunta pregunta;

    public Activity() : base() {}
	//Generico para menos bulla visual
	public Activity(string type, Vector3Data position, float scale, bool physics, Pregunta pregunta)
    : base(type, position, scale, physics)
	{
		this.pregunta = pregunta;
	}

	// Generico con string rotation
    public Activity(string type, Vector3Data position, float scale, string rotation, bool physics, Pregunta pregunta)
    :base(type, position, scale, rotation, physics)
	{        
        this.pregunta = pregunta;
    }

	// Generico con Quaternion rotation
    public Activity(string type, Vector3Data position, float scale, Vector3Data rotation, bool physics, Pregunta pregunta)
    :base(type, position, scale, rotation, physics)
	{ 
        this.pregunta = pregunta;
    }

	// CASO NPC
	// Quaternion
    public Activity(string type, Vector3Data position, float scale, Vector3Data rotation, string dialog, bool physics, Pregunta pregunta)
    :base(type, position, scale, rotation, dialog, physics)
	{
        this.pregunta = pregunta;
    }

	// string
    public Activity(string type, Vector3Data position, float scale, string rotation, string dialog, bool physics, Pregunta pregunta)
    :base(type, position, scale, rotation, dialog, physics)
    {
        this.pregunta = pregunta;
    }


}
