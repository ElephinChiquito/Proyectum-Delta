using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using TMPro;
using UnityEngine.UI;

public class DoorBehaviour : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI UITextPosition;
    [SerializeField] Image xImage;
    [SerializeField] Camera cam;
    [SerializeField] DoorChapuza doorChapuza;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {

        this.transform.root.gameObject.SetActive(false);
        xImage.gameObject.SetActive(false);

        int doorNumber = int.Parse(this.transform.root.name.Substring(5));
        if (DoorExists.exists(new Vector3(0, 0, doorNumber)))
        {
            this.transform.root.gameObject.SetActive(true);
            xImage.gameObject.SetActive(true);
        }
    }

    public void ToggleShow()
    {
        Vector3 selectedDoor = new Vector3(Map2D.x, Map2D.y, WhichDoor());
        if (this.transform.root.gameObject.activeSelf)
        {
            this.transform.root.gameObject.SetActive(false);
            xImage.gameObject.SetActive(false);
            DoorExists.remove(selectedDoor);
        }
        else
        {
            this.transform.root.gameObject.SetActive(true);
            xImage.gameObject.SetActive(true);
            DoorExists.add(selectedDoor);
        }
    }

    public int WhichDoor() {
        switch (this.transform.root.gameObject.name)
        {
            case "door 1": return 1;
            case "door 4": return 4;
            case "door 3": return 3;
            case "door 2": return 2;
            default: return -1;
        }
    }

    public string WhichDoorString()
    {
        switch (WhichDoor())
        {
            case 1: return "north";
            case 2: return "east";
            case 3: return "west";
            case 4: return "south";
            default: return "north";
        }
    }

    public void Travel()
    {
        GeneradorDeEscena3D.SaveScene();
        GeneradorDeEscena3D.CleanScene();

        
        Vector3 camOffsetTravelNorth = new Vector3(0.000000f, 1.824083f, -10.39104f); // 0.000000  1.824083 -10.39104
        Vector3 camOffsetTravelSouth = new Vector3(0.000000f, 1.824083f, 10.74686f);  // 0.000000  1.824083  10.74686
        Vector3 camOffsetTravelWest = new Vector3(10.74686f, 1.824083f, 0.000000f);   // 10.74686  1.824083  0.000000
        Vector3 camOffsetTravelEast = new Vector3(-10.39104f,  1.824083f,  0.000000f); //-10.39104  1.824083  0.000000

        Quaternion camRotationTravelNorth = Quaternion.LookRotation(Vector3.forward);
        Quaternion camRotationTravelSouth = Quaternion.LookRotation(Vector3.back);
        Quaternion camRotationTravelEast = Quaternion.LookRotation(Vector3.right);
        Quaternion camRotationTravelWest = Quaternion.LookRotation(Vector3.left);

        switch (this.transform.root.gameObject.name)
        {
            case "door 1":
                Map2D.y++;
                cam.transform.root.position = new Vector3(0, 0, 0) + camOffsetTravelNorth;
                cam.transform.root.rotation = camRotationTravelNorth;
                break;
            case "door 4":
                Map2D.y--;
                cam.transform.root.position = new Vector3(0, 0, 0) + camOffsetTravelSouth;
                cam.transform.root.rotation = camRotationTravelSouth;
                break;
            case "door 3":
                Map2D.x++;
                cam.transform.root.position = new Vector3(0, 0, 0) + camOffsetTravelEast;
                cam.transform.root.rotation = camRotationTravelEast;
                break;
            case "door 2":
                Map2D.x--;
                cam.transform.root.position = new Vector3(0, 0, 0) + camOffsetTravelWest;
                cam.transform.root.rotation = camRotationTravelWest;
                break;
        }

        string ruta = Path.Combine(Application.persistentDataPath, "escena " + Map2D.x + "_" + Map2D.y + ".json");
        if (File.Exists(ruta))
        {
            GeneradorDeEscena3D.CrearEscena(Map2D.x, Map2D.y);
        }
        UITextPosition.text = "ROOM x:" + Map2D.x + " y:" + Map2D.y;

        GameObject[] doors = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in doors)
        {
            GameObject aux = obj.transform.root.gameObject;

            if (obj.transform.name.Equals("door 1") || obj.transform.name.Equals("door 2") || obj.transform.name.Equals("door 3") || obj.transform.name.Equals("door 4"))
            {
                int doorNumber = int.Parse(obj.transform.name.Substring(5)); // Desde el carácter 5 en adelante
                aux.SetActive(false);
                doorChapuza.setXImage(false, doorNumber);

                if (DoorExists.exists(new Vector3(Map2D.x, Map2D.y, doorNumber)))
                {
                    aux.SetActive(true);
                    doorChapuza.setXImage(true, doorNumber);
                }

                Debug.Log("Puerta " + doorNumber + " Existe ? " + DoorExists.exists(new Vector3(Map2D.x, Map2D.y, doorNumber)));
            }
            if (obj.transform.root.name.Equals("XR Origin (XR Rig)"))
                obj.transform.SetPositionAndRotation(new Vector3(0, 0.1f, 0), Quaternion.identity);
        }
    }
}
