using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Vector3ListWrapper
{
    public List<Vector3> list;
}

public class DoorExists : MonoBehaviour
{
    public static List<Vector3> doors = new List<Vector3>();
    private static string folderPath;
    private static string filePath;

    void Start()
    {
        folderPath = Path.Combine(Application.persistentDataPath, "PermanentInfo");
        filePath = Path.Combine(folderPath, "DoorList.json");
        Directory.CreateDirectory(folderPath);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Vector3ListWrapper wrapper = JsonUtility.FromJson<Vector3ListWrapper>(json);

            if (wrapper != null && wrapper.list != null)
            {
                doors = wrapper.list;
                Debug.Log("Puertas cargadas: " + doors.Count);
            }
            else
            {
                Debug.LogWarning("Archivo JSON vacío o malformado.");
                doors = new List<Vector3>();
            }
        }
        else
        {
            doors = new List<Vector3>();
            SaveDoors(filePath); // crear archivo vacío
        }
    }

    public static bool exists(Vector3 xyz) => doors.Contains(xyz);

    public static void add(Vector3 xyz)
    {
        if (!doors.Contains(xyz))
        {
            doors.Add(xyz);
            switch (xyz.z)
            {
                case 1: doors.Add(new Vector3(xyz.x, xyz.y + 1, 4)); break;
                case 2: doors.Add(new Vector3(xyz.x - 1, xyz.y, 3)); break;
                case 3: doors.Add(new Vector3(xyz.x + 1, xyz.y, 2)); break;
                case 4: doors.Add(new Vector3(xyz.x, xyz.y - 1, 1)); break;
            }
            SaveDoors(Path.Combine(Application.persistentDataPath, "PermanentInfo", "DoorList.json"));
        }
    }
    public static void remove(Vector3 xyz)
    {
        if (doors.Contains(xyz))
        {
            doors.Remove(xyz);
            switch (xyz.z)
            {
                case 1: doors.Remove(new Vector3(xyz.x, xyz.y + 1, 4)); break;
                case 2: doors.Remove(new Vector3(xyz.x - 1, xyz.y, 3)); break;
                case 3: doors.Remove(new Vector3(xyz.x + 1, xyz.y, 2)); break;
                case 4: doors.Remove(new Vector3(xyz.x, xyz.y - 1, 1)); break;
            }
            SaveDoors(filePath);
        }
    }

    private static void SaveDoors(string filePath)
    {
        Vector3ListWrapper wrapper = new Vector3ListWrapper { list = doors };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Guardado DoorList.json con " + doors.Count + " puertas.");
    }
}
