using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private UnityEvent _PlayerDead;
    
    private List<PlayerHealth> _players = new List<PlayerHealth>();
    
    
    public void AddPlayer(PlayerHealth player)
    {
        _players.Add(player);
        player.OnDead += OnDead;
    }

    private void OnDead(PlayerHealth player)
    {
        //Debug.Log("Enemy dead : " + enemy.name);
        
        if(player != null) 
            _players.Remove(player);
                
        if(_players.Count <= 0)
        {
            _PlayerDead?.Invoke();
            
        }
        
    }
}
