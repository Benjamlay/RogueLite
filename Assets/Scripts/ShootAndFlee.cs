using System.Collections;
using UnityEngine;

public class ShootAndFlee : MonoBehaviour
{
    private EnemyBehaviour _enemyBehaviour;
    [SerializeField] private GameObject _arrowPrefab;

    private ArrowBehaviour Arrow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    
    private void Awake()
    {
        _enemyBehaviour = GetComponent<EnemyBehaviour>();
    }
    
    void Start()
    {
        InvokeRepeating("updateState", 0f, 1f);
        
    }

    // Update is called once per frame


    private void updateState()
    {
        if (_enemyBehaviour._enemyDetection._detected)
        {
            //tirer sur le joueur
            Debug.Log("start shooting the player");
            StartCoroutine(_enemyBehaviour.Shoot(_arrowPrefab));
            
            if (_enemyBehaviour._enemyDetection._PlayerTooClose)
            {
                Debug.Log("fleeing the player");
                StartCoroutine(_enemyBehaviour.Flee());
            }
            
        }
        else if(_enemyBehaviour._enemyDetection._detected == false)
        {
            StartCoroutine(_enemyBehaviour.Patrol());
        }
    }

    
    void Update()
    {
        
    }

    
}
