using UnityEngine;

public class MapManager : MonoBehaviour
{
    
    [SerializeField] GeneratorPCG generator;
    [SerializeField] GameObject loadingScreen;
    
    [SerializeField] private bool WinTheMap = false;

    [SerializeField] private GameObject Boat;
    
    PlayerMovement player;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadingScreen.SetActive(false);
        player = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (WinTheMap)
        {
            loadingScreen.SetActive(true);
            generator.ClearMap();
            generator.StartGenerate();
            player.SetPosition(new Vector2(0, 0));
            WinTheMap = false;
        }
        loadingScreen.SetActive(false);
    }

    public void PlayerWon()
    {
        Debug.Log("Player Won");
        Instantiate(Boat, generator.BoatSpawn(), Quaternion.identity);
        // if(/*le joueur monte sur le bateau*/)
        
            //WinTheMap = true;
        
    }

    public void PlayerLost()
    {
        Debug.Log("Player Lost");
    }
}
