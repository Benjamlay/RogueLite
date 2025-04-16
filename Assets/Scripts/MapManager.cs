using UnityEngine;

public class MapManager : MonoBehaviour
{
    
    [SerializeField] GeneratorPCG generator;
    [SerializeField] GameObject loadingScreen;
    
    [SerializeField] private bool WinTheMap = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadingScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (WinTheMap)
        {
            loadingScreen.SetActive(true);
            generator.ClearMap();
            generator.StartGenerate();
            WinTheMap = false;
        }
        loadingScreen.SetActive(false);
    }
}
