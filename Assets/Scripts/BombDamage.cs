using System;
using UnityEngine;

public class BombDamage : MonoBehaviour
{
    private PlayerHealth _player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _player.TakeDamage(2, transform.position);
        }
    }
}
