using System;

[Serializable]
public class Vector3Data
{
    public float x;
    public float y;
    public float z;

    // Constructor por comodidad
    public Vector3Data(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    // Conversión a Vector3 de Unity
    public UnityEngine.Vector3 ToVector3()
    {
        return new UnityEngine.Vector3(x, y, z);
    }

    // Conversión desde Vector3 de Unity (opcional)
    public static Vector3Data FromVector3(UnityEngine.Vector3 vector)
    {
        return new Vector3Data(vector.x, vector.y, vector.z);
    }
}
