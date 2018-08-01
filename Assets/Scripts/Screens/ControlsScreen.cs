using UnityEngine;
using System.Collections;

public class ControlsScreen : MonoBehaviour 
{
    private bool isShowing;
    public bool IsShowing
    {
        get
        {
            return isShowing;   
        }
    }

    public GameObject ControlHelpCanvas;

    public void Close()
    {
        isShowing = false;
    }

    public void Show()
    {
        isShowing = true;
    }

    void Update()
    {
        ControlHelpCanvas.SetActive(isShowing);
    }
}
