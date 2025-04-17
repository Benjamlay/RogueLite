using UnityEngine;

public class BoatBehaviour : MonoBehaviour
{
    [SerializeField] private SpriteRenderer PlayerSprite;
    
    private bool playerOnBoat = false;
    void Start()
    {
        PlayerSprite.enabled = false;
    }

    
    void Update()
    {
        if (playerOnBoat)
        {
            transform.Translate(Vector2.right * (2f * Time.deltaTime));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerSprite.enabled = true;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.SetActive(false);
            playerOnBoat = true;
        }
    }
}
