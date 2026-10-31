using UnityEngine;

public class HideToolTipToggle : MonoBehaviour
{
    [SerializeField] GameObject toolTip;
    private bool wasToolTipHidden = false;
    
    public void toggleToolTipVisibility()
    {
        wasToolTipHidden = toolTip.activeSelf;

        toolTip.SetActive(!toolTip.activeSelf);
    }

    public void setToolTipVisibility(bool x, bool forceSet)
    {
        toolTip.SetActive(x && (!wasToolTipHidden || forceSet));
    }

}
