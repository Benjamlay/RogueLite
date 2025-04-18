using System.Collections;
using UnityEngine;

public class BoatBehaviour : MonoBehaviour
{
    [SerializeField] private SpriteRenderer PlayerSprite;
    
    public bool playerOnBoat = false;

    private GameObject player;
    
    MapManager mapManager;
    void Start()
    {
        PlayerSprite.enabled = false;
        mapManager = FindObjectOfType<MapManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    
    void Update()
    {
        if (playerOnBoat)
        {
            transform.Translate(Vector2.left * (2f * Time.deltaTime));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerSprite.enabled = true;
            player.SetActive(false);
            playerOnBoat = true;
            StartCoroutine("OnToTheNextLevel");
        }
    }

    public IEnumerator OnToTheNextLevel()
    {
        yield return new WaitForSeconds(4f);
        playerOnBoat = false;
        mapManager.NewMap();
        player.SetActive(true);
        Destroy(gameObject);
        
    }
}
