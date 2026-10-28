using System.Collections.Generic;
using UnityEngine;

public class RoomSorroundings : MonoBehaviour
{
	[SerializeField] GameObject surroundingRooms;
	private List<GameObject> roomsList;
	
	
	public void Start()
    {
		Debug.Log("starting RoomSorrounding and roomsList");
		roomsList = new List<GameObject>();
		foreach (Transform child in surroundingRooms.transform)
		{
			roomsList.Add(child.gameObject);
		}
    }

	public void setSorroundingRoom(int roomNumber)
	{
		if(roomsList == null){ Debug.Log("roomList IS NULL"); }
		foreach (GameObject child in roomsList)
		{
			child.SetActive(false);
		}
		roomsList[roomNumber].SetActive(true);
		VariablesLoader.Data.roomNumber = roomNumber;
		VariablesLoader.Save();
	}

}
