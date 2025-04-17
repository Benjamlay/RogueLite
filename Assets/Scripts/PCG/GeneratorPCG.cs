using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GeneratorPCG : MonoBehaviour
{
    [SerializeField] public Tilemap islandMap;
    [SerializeField] private Tilemap WaterMap;
    [SerializeField] private Tilemap AnimatedWaterMap;
    [SerializeField] private List<TileBase> tiles;
    [SerializeField] private Vector3Int startPosition = Vector3Int.zero;

    [SerializeField] private TileBase waterTile;
    [SerializeField] private TileBase animatedWaterTile;
    
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private float treeDensity = 0.2f;
    public List<GameObject> AllTrees = new List<GameObject>();
    
    [SerializeField] private GameObject HousePrefab;
    [SerializeField] private float houseDensity = 0.2f;
    
    [SerializeField] private GameObject towerPrefab;
    
    [Header("Settings")]
    [SerializeField] private int lMin;
    [SerializeField] private int lMax;
    [SerializeField] private int iterMax;
    [SerializeField] private int nbTilesMax;
    [SerializeField] private int heightMax;
    [SerializeField] private int widthMax;

    [Header("Game of life limits")]
    [SerializeField] private int deadLimitMax;
    [SerializeField] private int deadLimitMin;
    [SerializeField] private int aliveLimit;
    [SerializeField] private int fillIterations;
    
    [SerializeField] private GameObject ArcherPrefab;
    [SerializeField] private GameObject ChaserPrefab;
    [SerializeField] private GameObject KamikasePrefab;
    
    [SerializeField] private float enemyDensity;
    
    public List<GameObject> Archers = new List<GameObject>();
    
    public bool generationCoroutine = true;
    
    private Vector2Int[] _directions = new[]
    {
        Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
    };
    private Vector3Int[] _mooreNeighbours = new[]
    {
        new Vector3Int(0, 1, 0),
        new Vector3Int(1, 1, 0),
        new Vector3Int(1, 0, 0),
        new Vector3Int(1, -1, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(-1, -1, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(-1, 1, 0)
    };

    private BoundsInt _barrier;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetBarrier();
        ClearMap();
        StartGenerate();
    }
    private void OnDrawGizmos()
    {

        SetBarrier();

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_barrier.center, _barrier.size);
    }
    private void SetBarrier()
    {

        Vector3Int size = new Vector3Int(widthMax, heightMax, 0);
        Vector3Int boundsPosition = startPosition - size / 2;

        _barrier = new BoundsInt(boundsPosition, size);
    }
    public void StartGenerate()
    {
        if(generationCoroutine)
        {
            Generate();
            AddingWaterAndTrees();
            FillWithAnimatedWater();
            FillWithEnemies();
        }

        StartCoroutine("AStarUpdate");
    }
    private IEnumerator AStarUpdate()
    {
        yield return new WaitForSeconds(0.2f);
        AstarPath.active.Scan();
    }
    private void Generate()
    {
        generationCoroutine = false;
        GenerateDrunkard();
        GameOfLife();
        
        generationCoroutine = true;
    }
    public void ClearMap()
    {
        islandMap.ClearAllTiles();
        WaterMap.ClearAllTiles();
        AnimatedWaterMap.ClearAllTiles();
        foreach (GameObject tree in AllTrees)
        {
            DestroyImmediate(tree);
        }
        AllTrees.Clear();
        
        foreach (GameObject archers in Archers)
        {
            DestroyImmediate(archers);
        }
        Archers.Clear();
        
    }
    private void GenerateDrunkard()
    {
        Vector2Int direction = Vector2Int.zero;
        Vector3Int position = Vector3Int.zero;

        int tileCount = 0;
        int iterCount = 0;

        AddTile(startPosition, ref tileCount);

        while (tileCount < nbTilesMax && iterCount < iterMax)
        {
            direction = _directions[Random.Range(0, _directions.Length)];
            int currentPathLength = Random.Range(lMin, lMax);

            Vector3Int futurePosition = position + currentPathLength * new Vector3Int(direction.x, direction.y, 0);

            if (IsInBounds(futurePosition))
            {
                for (int i = 0; i < currentPathLength; i++)
                {
                    position += new Vector3Int(direction.x, direction.y, 0);
                    AddTile(position, ref tileCount);
                }

                iterCount++;

            }

            //yield return new WaitForSeconds(0.00001f);
            //Debug.Log($"Tile Count {tileCount} : Iter Count {iterCount}");

        }

    }
    private void GameOfLife()
    {
        List<Vector3Int> aliveCells = new List<Vector3Int>();
        List<Vector3Int> deadCells = new List<Vector3Int>();

        for(int nbIter = 0; nbIter < fillIterations; nbIter++)
        {
            aliveCells.Clear();
            deadCells.Clear();
            
            for (int x = _barrier.xMin; x < _barrier.xMax; x++)
            {
                for (int y = _barrier.yMin; y < _barrier.yMax; y++)
                {
                    

                    Vector3Int position = new Vector3Int(x, y);

                   
                    bool isAlive = islandMap.HasTile(position);

                    
                    int countAlive = CountAlive(position, islandMap);

                    if (isAlive)
                    {
                       
                    }
                    else
                    {
                        // Elle est morte
                        if (countAlive >= aliveLimit + nbIter)
                        {
                            // Elle devient vivante
                            aliveCells.Add(position);
                        }
                        else
                        {
                            // sinon Elle reste morte
                            deadCells.Add(position);
                        }
                    }
                }
            }
            foreach (Vector3Int aliveCell in aliveCells)
            {
                if (!islandMap.HasTile(aliveCell))
                {
                    islandMap.SetTile(aliveCell, GetRandomTile());
                }
            }
            foreach (Vector3Int deadCell in deadCells)
            {
                if (islandMap.HasTile(deadCell)) islandMap.SetTile(deadCell, null);
            }
        }
    }
    private int CountAlive(Vector3Int position, Tilemap map)
    {
        int countAlive = 0;
        foreach (Vector3Int neighbour in _mooreNeighbours)
        {
            if (IsInBounds(position + neighbour))
            {
                TileBase t = map.GetTile(position + neighbour);
                if (t != null)
                {
                    // il y a bien une tile a cet emplacement
                    countAlive++;
                }
            }
        }

        return countAlive;
    }
    private void AddTile(Vector3Int position, ref int count)
    {

        if (islandMap.HasTile(position))
            return;
        
        islandMap.SetTile(position, GetRandomTile());
        count++;

    }
    private TileBase GetRandomTile()
    {
        return tiles[Random.Range(0, tiles.Count)];
    }
    private bool IsInBounds(Vector3Int position)
    {
        return (position.x >= _barrier.xMin && position.x < _barrier.xMax && position.y >= _barrier.yMin &&
                position.y < _barrier.yMax);
    }
    private void AddingWaterAndTrees()
    {
        for (int x = _barrier.xMin; x < _barrier.xMax; x++)
        {
            for (int y = _barrier.yMin; y < _barrier.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y);
                
                
                    if (!islandMap.HasTile(position))
                    {
                        WaterMap.SetTile(position, waterTile);
                    }
                    
                    if (islandMap.HasTile(position) && CountAlive(position, islandMap) == 8)
                    {
                        if (Random.Range(0f, 1f) < treeDensity)
                        {
                            GameObject NewTree = Instantiate(treePrefab, position, Quaternion.identity);
                            AllTrees.Add(NewTree);
                        }
                    }

                    if (islandMap.HasTile(position) && CountAlive(position, islandMap) == 8)
                    {
                        if (Random.Range(0f, 1f) < houseDensity)
                        {
                            GameObject NewHouse = Instantiate(HousePrefab, position, Quaternion.identity);
                            AllTrees.Add(NewHouse);
                        }
                    }
            }
        }
    }
    private void FillWithAnimatedWater()
    {
        for (int x = _barrier.xMin; x < _barrier.xMax; x++)
        {
            for (int y = _barrier.yMin; y < _barrier.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y);

                if (!WaterMap.HasTile(position))
                {
                    int countAlive = CountAlive(position, WaterMap);
                    if (countAlive >= 1)
                    {
                        AnimatedWaterMap.SetTile(position, animatedWaterTile);
                    }
                }
                
            }
        }
    }
    public void FillWithEnemies()
    {
        for (int x = _barrier.xMin; x < _barrier.xMax; x++)
        {
            for (int y = _barrier.yMin; y < _barrier.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y);
                
                if (islandMap.HasTile(position) && CountAlive(position, islandMap) > 7)
                {
                    if (Random.Range(0f, 1f) < enemyDensity)
                    {
                        GameObject newArcher =Instantiate(ArcherPrefab, position, Quaternion.identity);
                        Archers.Add(newArcher);
                        EnemyManager eManager = FindAnyObjectByType<EnemyManager>();
                        eManager.AddEnemy(newArcher.GetComponent<EnemyHealthPoints>());
                    }
                }
            }
        }
    }
    public Vector2 BoatSpawn()
    {
        for (int x = _barrier.xMin; x < _barrier.xMax; x++)
        {
            for (int y = _barrier.yMin; y < _barrier.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y);
                if(WaterMap.HasTile(position) && CountAlive(position, WaterMap) < 8 && CountAlive(position, islandMap) > 0)
                {
                    return new Vector2(WaterMap.CellToWorld(position).x, WaterMap.CellToWorld(position).y);
                }
            }
        }

        return Vector2.zero;
    }
}
