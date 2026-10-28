using System.Collections.Generic;
using Neocortex.Samples;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI objectList;
    [SerializeField] TMP_InputField input;
    List<string> AllObjectsList = ChatSample.AllObjectsList;

    public void Reload()
    {
        if (input.text == "" || input.text == null || input.text.Length < 1) { objectList.text = "List: " + string.Join(", ", AllObjectsList); Debug.Log(string.Join(", ", AllObjectsList)); }
        else
        {
            objectList.text = "List: ";
            for (int i = 0; i < AllObjectsList.Count; i++)
            {
                if (AllObjectsList[i].ToLower().Contains(input.text.ToLower()))
                {
                    objectList.text += AllObjectsList[i] + ", ";
                }
            }
            if (objectList.text.Length > 6)
                objectList.text = objectList.text.Substring(0, objectList.text.Length - 2);
        }
    }
}
