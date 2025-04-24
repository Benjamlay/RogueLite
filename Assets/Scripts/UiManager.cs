using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.Rendering;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Image panel;
    
    private bool PanelOpened = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panel.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpenCloseUI(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            panel.enabled = true;
        }
    }
}
