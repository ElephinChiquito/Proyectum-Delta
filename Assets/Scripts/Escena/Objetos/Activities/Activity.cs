using System;
using UnityEngine;


[Serializable]
public class Activity: Objeto3D
{
	

    public Activity() : base() {}
	//Generico para menos bulla visual
	
	public Activity(string type, Vector3Data position, float scale, bool physics)
    : base(type, position, scale, physics)
	{}

	public Activity(string type, Vector3Data position, float scale, string rotation, bool physics)
    :base(type, position, scale, rotation, physics)
	{}

	public Activity(string type, Vector3Data position, float scale, Vector3Data rotation, bool physics)
    :base(type, position, scale, rotation, physics)
	{}
}

[Serializable]
public class TestActivityData : Activity
{
	// Todos estos parametros son realmente de la actividad tipo test
	// Pero manejada como objeto 3D
    public Pregunta[] questions;
	public const string TYPE = "TestActivity";

	public TestActivityData() : base() {type = TYPE;}

	public TestActivityData(Vector3Data position, float scale, bool physics, Pregunta[] questions)
    : base(TYPE, position, scale, physics)
	{
        this.questions = questions;
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
    public TestActivityData(Vector3Data position, float scale, string rotation, bool physics, Pregunta[] questions)
    :base(TYPE, position, scale, rotation, physics)
	{
        this.questions = questions;
    }

	// Generico con Quaternion rotation
    public TestActivityData(Vector3Data position, float scale, Vector3Data rotation, bool physics, Pregunta[] questions)
    :base(TYPE, position, scale, rotation, physics)
	{ 
        this.questions = questions;
	}
}
