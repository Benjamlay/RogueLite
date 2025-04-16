using System;
using UnityEngine;

public class EnemyRandomSpawn : MonoBehaviour
{
    private GeneratorPCG generator;

    [SerializeField] public int neighboursNeeded = 7;
    [SerializeField] public float enemyDensity = 0.01f;

    [SerializeField] private GameObject ArcherPrefab;
    [SerializeField] private GameObject ChaserPrefab;
    [SerializeField] private GameObject KamikasePrefab;


    void Awake()
    {
        generator = GetComponent<GeneratorPCG>();
    }
    void Start()
    {
        //SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
