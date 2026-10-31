using UnityEngine;
using UnityEngine.EventSystems;

public class HandCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D handCursor; // Asigna una textura de mano en el Inspector

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(handCursor, new Vector2(8, 0), CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}