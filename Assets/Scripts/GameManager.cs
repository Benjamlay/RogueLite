using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PLAY()
    {
        SceneManager.LoadScene("PCG_scene");
    }

    public void QUIT()
    {
        Application.Quit();
    }
}
