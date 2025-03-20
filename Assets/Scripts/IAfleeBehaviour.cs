// using UnityEngine;
// using System.Collections.Generic;
//
// public class IAfleeBehaviour : MonoBehaviour
// {
//     public Transform player;
//     public float moveSpeed = 3f;
//     
//     private Vector2Int enemyPos;
//     private Vector2Int playerPos;
//     private Pathfinding pathfinding;  // Référence à ton A*
//     private Grid grid;  // Référence à ta Grid
//
//     private void Start()
//     {
//         pathfinding = FindObjectOfType<Pathfinding>();  // Récupérer A*
//         grid = FindObjectOfType<Grid>();  // Récupérer la grille existante
//     }
//
//     private void Update()
//     {
//         enemyPos = grid.WorldToGridPosition(transform.position);
//         playerPos = grid.WorldToGridPosition(player.position);
//
//         Vector2Int bestEscapePos = FindBestEscapePosition();
//         List<Vector2Int> path = pathfinding.FindPath(enemyPos, bestEscapePos);
//
//         if (path != null && path.Count > 1)
//         {
//             MoveTo(path[1]); // Se déplacer vers la prochaine case
//         }
//     }
//
//     private Vector2Int FindBestEscapePosition()
//     {
//         List<Vector2Int> possibleMoves = grid.GetNeighbors(enemyPos); // Utilise ta propre `Grid`
//         Vector2Int bestPos = enemyPos;
//         float maxDistance = Vector2.Distance(enemyPos, playerPos);
//
//         foreach (var move in possibleMoves)
//         {
//             float distanceToPlayer = Vector2.Distance(move, playerPos);
//             if (distanceToPlayer > maxDistance)
//             {
//                 maxDistance = distanceToPlayer;
//                 bestPos = move;
//             }
//         }
//
//         return bestPos;
//     }
//
//     private void MoveTo(Vector2Int gridPos)
//     {
//         Vector3 worldPos = grid.GridToWorldPosition(gridPos); // Convertir en coordonnées monde
//         transform.position = Vector3.MoveTowards(transform.position, worldPos, moveSpeed * Time.deltaTime);
//     }
// }
//
// }
//
