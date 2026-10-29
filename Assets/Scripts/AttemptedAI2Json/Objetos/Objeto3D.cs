using System;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;


[Serializable]
public class Objeto3D
{
    public string type;
    public Vector3Data position;
    public float scale;
    public Quaternion rotation;
    public string dialog;
    public bool physics;
	public string[] test;

	private void setRotationWithString(string rotation)
	{
		switch (rotation.ToLower())
        {
            case "north":
                this.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case "east":
                this.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case "south":
                this.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case "west":
                this.rotation = Quaternion.Euler(0, 270, 0);
                break;
            default:
                this.rotation = Quaternion.Euler(0, 0, 0);
                break;
        }
	}

    public Objeto3D(){}
	//Generico para menos bulla visual
	public Objeto3D(string type, Vector3Data position, float scale, bool physics)
	{
		this.type = type;
        this.position = position;
        this.scale = scale;
        this.physics = physics;
	}

	// Generico con string rotation
    public Objeto3D(string type, Vector3Data position, float scale, string rotation, bool physics)
    :this(type, position, scale, physics)
	{        
        setRotationWithString(rotation); 
    }

	// Generico con Quaternion rotation
    public Objeto3D(string type, Vector3Data position, float scale, Vector3Data rotation, bool physics)
    :this(type, position, scale, physics)
	{ 
        this.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    }

	// CASO NPC
	// Quaternion
    public Objeto3D(string type, Vector3Data position, float scale, Vector3Data rotation, string dialog, bool physics)
    :this(type, position, scale, rotation, physics)
	{
        this.dialog = dialog;
    }

	// string
    public Objeto3D(string type, Vector3Data position, float scale, string rotation, string dialog, bool physics)
    :this(type, position, scale, rotation, physics)
    {
        this.dialog = dialog;
    }


}
