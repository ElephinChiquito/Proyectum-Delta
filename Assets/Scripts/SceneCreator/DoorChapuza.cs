using UnityEngine;
using UnityEngine.UI;

public class DoorChapuza : MonoBehaviour
{
    [SerializeField] Image xImageDoor1;
    [SerializeField] Image xImageDoor2;
    [SerializeField] Image xImageDoor3;
    [SerializeField] Image xImageDoor4;
    public void setXImage(bool state, int doorNumber)
    {
        switch (doorNumber)
        {
            case 1:
                xImageDoor1.gameObject.SetActive(state);
                break;
            case 2:
                xImageDoor2.gameObject.SetActive(state);
                break;
            case 3:
                xImageDoor3.gameObject.SetActive(state);
                break;
            case 4:
                xImageDoor4.gameObject.SetActive(state);
                break;
        }
    }
}
