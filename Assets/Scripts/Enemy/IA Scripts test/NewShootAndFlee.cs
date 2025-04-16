using System.Collections;
using UnityEngine;

public class NewShootAndFlee : MonoBehaviour
{
    private EnemyBehaviour _enemyBehaviour;
    [SerializeField] private GameObject _arrowPrefab;
    private ArrowBehaviour Arrow;
    private Coroutine _coroutine;
    private Coroutine _fleeCoroutine;
    
    private bool shooting;
    private bool fleeing;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        ShootOrFlee();
        if (!_enemyBehaviour.StartCoroutine)
        {
            _coroutine = StartCoroutine(shooting ? _enemyBehaviour.Shoot(_arrowPrefab) : _enemyBehaviour.Patrol());
            
            if(fleeing)
            {
                if(_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                    _enemyBehaviour.StartCoroutine = false;
                }
                _fleeCoroutine = StartCoroutine(_enemyBehaviour.Flee());
            }
            else
            {
                StopCoroutine(_fleeCoroutine);
            }
        }
    }
    
    private void ShootOrFlee()
    {
        if (_enemyBehaviour._enemyDetection._detected)
        {
            shooting = true;
        }
        else if(_enemyBehaviour._enemyDetection._detected == false)
        {
            shooting = false;
        }
        fleeing = _enemyBehaviour._enemyDetection._PlayerTooClose;
    }
    
}
