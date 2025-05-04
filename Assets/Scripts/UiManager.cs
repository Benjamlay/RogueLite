using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
//using UnityEngine.Rendering;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button ResumeButton;
    [SerializeField] private Button RestartButton;
    [SerializeField] private Button QuitButton;
    
    private bool PanelOpened;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PanelOpened)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }


    public void OpenCloseUI(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            PanelOpened = !PanelOpened;
            Debug.Log("Escape pressed");
        }
    }

    public void Pause()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
        PanelOpened = true;
    }

    public void Resume()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
        PanelOpened = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
