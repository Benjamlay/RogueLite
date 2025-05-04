using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
//using UnityEngine.Rendering;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject GameOverPanel;
    private PlayerHealth _playerHealth;
    private bool PanelOpened;
    private bool DisplayingGameOver;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        panel.SetActive(false);
        GameOverPanel.SetActive(false);
        DisplayingGameOver = false;
    }
    
    
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

        if (_playerHealth.gameOver)
        {
            StartCoroutine("OpeningGameOverPanel");

            if(DisplayingGameOver)
            {
                GameOverPanel.SetActive(true);
                //Time.timeScale = 0f;
            }
        }
    }

    IEnumerator OpeningGameOverPanel()
    {
        yield return new WaitForSeconds(3f);
        DisplayingGameOver = true;
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
        GameOverPanel.SetActive(false);
    }
}
