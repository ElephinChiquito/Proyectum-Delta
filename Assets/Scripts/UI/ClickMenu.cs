using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickMenu : MonoBehaviour
{
	[SerializeField] FlyCamera flyCamera;
	private GameObject go;
	[SerializeField] GameObject NPCDialogBox;
	[SerializeField] TextMeshProUGUI NPCText;
	[SerializeField] TextMeshProUGUI title;
	[SerializeField] Camera cam;
	[SerializeField] TextMeshProUGUI movementModeButton;
	[SerializeField] TextMeshProUGUI holdModeButton;
	[SerializeField] Toggle toggle;
	[SerializeField] GameObject physicsToggle;

	public bool relativeMovementMode;
	public bool holdMovementMode;
	public float movementSensibility;

	string region;
    PhysicsVariable pv;

	public void StartClickMenu()
	{
		//Cambiar a config permanente en VariablesLoader
		relativeMovementMode = VariablesLoader.Data.movementType.Equals("relative");
		holdMovementMode = VariablesLoader.Data.movementHold.Equals("hold");
		movementSensibility = 0.5f;

		if (relativeMovementMode)
        	VariablesLoader.ChangeAccordingToLanguage(movementModeButton, "Relative", "Relativo");
		else
			VariablesLoader.ChangeAccordingToLanguage(movementModeButton, "Absolute", "Absoluto");

		if (holdMovementMode)
			VariablesLoader.ChangeAccordingToLanguage(holdModeButton, "Hold", "Mantener");
		else
			VariablesLoader.ChangeAccordingToLanguage(holdModeButton, "Not Hold", "No Mantener");
	}

	public void SelectGO(GameObject go)
	{
		this.go = go;
		
        pv = go.GetComponent<PhysicsVariable>();
		if(pv != null){ toggle.SetIsOnWithoutNotify(pv.GetValue()); }
		else{ Debug.Log("No tiene physics variable"); }

		title.text = go.transform.root.name.Substring(0, go.transform.root.name.Length - 7);

		//Regiones norte sur etc.
		Vector2 objectToCam = new Vector2(cam.transform.position.x - go.transform.position.x, cam.transform.position.z - go.transform.position.z);
		float camAngle = Mathf.Atan2(objectToCam.y, objectToCam.x) * Mathf.Rad2Deg;

		//realmente el norte esta en z+ pq no lo pense al principio esto es solo un apaño
		camAngle -= 90;
		if (camAngle < 0) camAngle += 360;

		if (camAngle >= 0 && camAngle <= 45)
		{
			region = "north";
		}
		else if (camAngle > 45 && camAngle <= 135)
		{
			region = "west"; // no se pq pero tiene q estar asi, pq sino esta al reves
		}
		else if (camAngle > 135 && camAngle <= 225)
		{
			region = "south";
		}
		else if (camAngle > 225 && camAngle <= 315)
		{
			region = "east";
		}
		else
		{
			region = "north";
		}
		Debug.Log("region = " + region + " camAngle = " + camAngle + "º objectToCam = " + objectToCam);

		//Tutorial Condition
		TutorialPopUp.SetFirstTimeClickMenuOpenTrue();

	}
	public void MoveX(float x)
	{
		if (relativeMovementMode)
		{
			switch (region)
			{
				case "north":
					go.transform.position += new Vector3(-x, 0, 0);
					break;
				case "west":
					go.transform.position += new Vector3(0, 0, -x);
					break;
				case "south":
					go.transform.position += new Vector3(x, 0, 0);
					break;
				case "east":
					go.transform.position += new Vector3(0, 0, x);
					break;
			}
		}
		else
			go.transform.position = go.transform.position + new Vector3(x, 0, 0);
	}
	public void MoveY(float y) { go.transform.position = go.transform.position + new Vector3(0, y, 0); }
	public void MoveZ(float z)
	{
		if (relativeMovementMode)
		{
			switch (region)
			{
				case "north":
					go.transform.position += new Vector3(0, 0, -z);
					break;
				case "west":
					go.transform.position += new Vector3(z, 0, 0);
					break;
				case "south":
					go.transform.position += new Vector3(0, 0, z);
					break;
				case "east":
					go.transform.position += new Vector3(-z, 0, 0);
					break;
			}
		}
		else
			go.transform.position = go.transform.position + new Vector3(0, 0, z);
	}

	public void changeMovementMode() {
		if (relativeMovementMode)
		{
			relativeMovementMode = false;
			VariablesLoader.Data.movementType = "absolute";
			VariablesLoader.Save();
			VariablesLoader.ChangeAccordingToLanguage(movementModeButton, "Absolute", "Absoluto");
		}
		else
		{
			relativeMovementMode = true;
			VariablesLoader.Data.movementType = "relative";
			VariablesLoader.Save();
			VariablesLoader.ChangeAccordingToLanguage(movementModeButton, "Relative", "Relativo");
		}
	}
	public void changeHoldMode() {
		if (holdMovementMode)
		{
			holdMovementMode = false;
			VariablesLoader.Data.movementHold = "not hold";
			VariablesLoader.Save();
			VariablesLoader.ChangeAccordingToLanguage(holdModeButton, "Not Hold", "No Mantener");
		}
		else
		{
			VariablesLoader.Data.movementHold = "hold";
			VariablesLoader.Save();
			holdMovementMode = true;
			VariablesLoader.ChangeAccordingToLanguage(holdModeButton, "Hold", "Mantener");
		}
	}
	public void setSensibility(float sensibility)
	{
		movementSensibility = sensibility;
	}

	public void Scale(float x) { go.transform.localScale = go.transform.localScale + new Vector3(go.transform.localScale.x * x, go.transform.localScale.y * x, go.transform.localScale.z * x); }
	public void RotateX(float x) {
		if (relativeMovementMode)
		{
			switch (region)
			{
				case "north":
					go.transform.Rotate(-x, 0, 0);
					break;
				case "west":
					go.transform.Rotate(0, 0, -x);
					break;
				case "south":
					go.transform.Rotate(x, 0, 0);
					break;
				case "east":
					go.transform.Rotate(0, 0, x);
					break;
			}
		}
		else
			go.transform.Rotate(x, 0, 0);
	}
	public void RotateY(float y) { go.transform.Rotate(0, y, 0); }
	public void RotateZ(float z) {
		if (relativeMovementMode)
		{
			switch (region)
			{
				case "north":
					go.transform.Rotate(0, 0, -z);
					break;
				case "west":
					go.transform.Rotate(-z, 0, 0);
					break;
				case "south":
					go.transform.Rotate(0, 0, z);
					break;
				case "east":
					go.transform.Rotate(z, 0, 0);
					break;
			}
		}
		else
			go.transform.Rotate(0, 0, z);
	}
	public void ResetRotation() { go.transform.rotation = Quaternion.identity; }
	public void ResetScale() { go.transform.localScale = Vector3.one; }


	public void Delete() { flyCamera.DestroyWithClickMenu(go); Unlock(); }
	public void Unlock()
	{
		flyCamera.UnlockAllForClickMenu();
		TutorialPopUp.SetFirstTimeClickMenuClosedTrue();
	}
	public void ShowNPCDialog(bool x) { NPCDialogBox.SetActive(x); }
	public void ShowPhysicsToggle(bool x){ physicsToggle.SetActive(x); }
	public void SetNPCDialogBox(string x) { NPCText.text = x; }

	//
	public void SetPhysics(bool x)
    {
		if(pv != null)
        {
			Debug.Log("Cambiando las fisicas a " + go.name + " | pv = " + x);
            pv.SetValue(x);
        }
		else
			Debug.LogError("Error, no hay Physics Variable en el gameobject seleccionado.");
    }
}
