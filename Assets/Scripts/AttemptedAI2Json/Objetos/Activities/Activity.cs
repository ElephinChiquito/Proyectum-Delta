using System;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;


[Serializable]
public class Activity: Objeto3D
{
    public Pregunta[] questions;

    public Activity() : base() {}
	//Generico para menos bulla visual
	public Activity(string type, Vector3Data position, float scale, bool physics, Pregunta[] questions)
    : base(type, position, scale, physics)
	{
		if(type.Equals("TestActivity"))
            this.questions = questions;
        else //default
            Debug.LogWarning("La actividad no tiene tipo reconocido type = {" + type + "}");
	}


    /// <summary>
    /// Este es el codigo para cargar la actividad con Pregunta del quiz
	/// Generico con string rotation
    /// </summary>
    /// <param name="type"></param>
    /// <param name="position"></param>
    /// <param name="scale"></param>
    /// <param name="rotation"></param>
    /// <param name="physics"></param>
    /// <param name="questions"></param>
    public Activity(string type, Vector3Data position, float scale, string rotation, bool physics, Pregunta[] questions)
    :base(type, position, scale, rotation, physics)
	{
        if(type.Equals("TestActivity"))
            this.questions = questions;
        else //default
            Debug.LogWarning("La actividad no tiene tipo reconocido type = {" + type + "}");
    }

	// Generico con Quaternion rotation
    public Activity(string type, Vector3Data position, float scale, Vector3Data rotation, bool physics, Pregunta[] questions)
    :base(type, position, scale, rotation, physics)
	{ 
        if(type.Equals("TestActivity"))
            this.questions = questions;
        else //default
            Debug.LogWarning("La actividad no tiene tipo reconocido type = {" + type + "}");
    }

    /* 
    
    "activity": {
		"type": "TestActivity",
		"position": { "x": 6, "y": 0, "z": 2 },
		"scale": 1,
		"rotation": "south",
		"physics": false,
		"questions": [
			{
				"question": "¿Qué absorben los árboles?",
				"options": ["Oxígeno", "Dióxido de carbono", "Helio", ""],
				"correctAnswer": 1
			},
			{
				"question": "¿Qué producen los árboles?",
				"options": ["Oxígeno", "Metano", "Hidrógeno", "Nitrógeno"],
				"correctAnswer": 0
			}
		]
	}
    
    */

}
