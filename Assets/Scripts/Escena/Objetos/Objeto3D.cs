using System;
using UnityEngine;

[Serializable]
public class Objeto3D
{
    public string type;
    public Vector3Data position;
    public float scale;
    public string rotation;            // "north"/"east"/"south"/"west" (lo escribe la IA)
    public Quaternion rotationExacta;  // lo escribe SaveScene
    public string dialog;
    public bool physics;
    public string[] test;

    // Rotación final que se aplica en escena
    public Quaternion Rotation
    {
        get
        {
            if (!string.IsNullOrEmpty(rotation))
            {
                switch (rotation.ToLower())
                {
                    case "east":  return Quaternion.Euler(0, 90, 0);
                    case "south": return Quaternion.Euler(0, 180, 0);
                    case "west":  return Quaternion.Euler(0, 270, 0);
                    default:      return Quaternion.identity;   // north y otros
                }
            }
            bool sinDato = rotationExacta.x == 0 && rotationExacta.y == 0
                        && rotationExacta.z == 0 && rotationExacta.w == 0;
            return sinDato ? Quaternion.identity : rotationExacta;
        }
    }

    public Objeto3D() {}

    public Objeto3D(string type, Vector3Data position, float scale, bool physics)
    {
        this.type = type; this.position = position;
        this.scale = scale; this.physics = physics;
    }

    public Objeto3D(string type, Vector3Data position, float scale, string rotation, bool physics)
    : this(type, position, scale, physics)
    { this.rotation = rotation; }

    public Objeto3D(string type, Vector3Data position, float scale, Vector3Data rotation, bool physics)
    : this(type, position, scale, physics)
    { rotationExacta = Quaternion.Euler(rotation.x, rotation.y, rotation.z); }

    public Objeto3D(string type, Vector3Data position, float scale, Vector3Data rotation, string dialog, bool physics)
    : this(type, position, scale, rotation, physics)
    { this.dialog = dialog; }

    public Objeto3D(string type, Vector3Data position, float scale, string rotation, string dialog, bool physics)
    : this(type, position, scale, rotation, physics)
    { this.dialog = dialog; }
}