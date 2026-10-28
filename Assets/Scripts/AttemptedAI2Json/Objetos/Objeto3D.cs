using System;
using System.Collections.Generic;
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

    public Objeto3D(){}
    public Objeto3D(string type, Vector3Data position, float scale, string rotation, bool physics)
    {
        this.type = type;
        this.position = position;
        this.scale = scale;
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
        this.physics = physics;
    }

    public Objeto3D(string type, Vector3Data position, float scale, Vector3Data rotation, bool physics)
    {
        this.type = type;
        this.position = position;
        this.scale = scale;
        this.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        this.physics = physics;
    }

    public Objeto3D(string type, Vector3Data position, float scale, Vector3Data rotation, string dialog, bool physics)
    {
        this.type = type;
        this.position = position;
        this.scale = scale;
        this.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        this.dialog = dialog;
        this.physics = physics;

    }

    public Objeto3D(string type, Vector3Data position, float scale, string rotation, string dialog, bool physics)
    {
        this.type = type;
        this.position = position;
        this.scale = scale;
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
        this.dialog = dialog;
        this.physics = physics;
    }
}
