using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private UnityEvent _onEnemiesCleared;
    
    private List<EnemyHealthPoints> _enemies = new List<EnemyHealthPoints>();
    
    
    public void AddEnemy(EnemyHealthPoints enemy)
    {
        _enemies.Add(enemy);
        enemy.OnDead += EnemyOnDead;
    }
    
    private void EnemyOnDead(EnemyHealthPoints enemy)
    {
        //Debug.Log("Enemy dead : " + enemy.name);
        
        if(enemy != null) 
            _enemies.Remove(enemy);
                
        if(_enemies.Count <= 0)
        {
            _onEnemiesCleared?.Invoke();
            //Debug.Log("Invoke EnemyClear");
        }
        
    }
}
