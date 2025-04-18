using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class MapManager : MonoBehaviour
{
    
    [SerializeField] GeneratorPCG generator;
    [SerializeField] GameObject loadingScreen;
    
    [SerializeField] public bool WinTheMap = false;

    [SerializeField] private GameObject Boat;
    
    PlayerMovement player;
    
    [SerializeField] BoatBehaviour boatBehaviour;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadingScreen.SetActive(false);
        player = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
        // if(Boat.GetComponent<BoatBehaviour>().playerOnBoat)
        // {
        //     Debug.Log("player is on the boat");
        //     WinTheMap = true;
        // }
        
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
        //Debug.Log("Player Won");
        Instantiate(Boat, generator.BoatSpawn(), Quaternion.identity);
        // if(Boat.GetComponent<BoatBehaviour>().playerOnBoat)
        // {
        //     Debug.Log("player is on the boat");
        //     WinTheMap = true;
        // }
        

    }

    public void PlayerLost()
    {
        Debug.Log("Player Lost");
    }

    public void NewMap()
    {
        loadingScreen.SetActive(true);
        generator.ClearMap();
        generator.StartGenerate();
        player.SetPosition(new Vector2(0, 0));
        WinTheMap = false;
    }
}
