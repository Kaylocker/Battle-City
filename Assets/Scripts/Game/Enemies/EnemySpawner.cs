using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private GameObject[] _spawnPoints;

    private Vector2 _target;
    private List<GameObject> _aliveEnemies = new List<GameObject>();
    private const float TIME_DELAY_SPAWNING = 5, TIME_DELAY_CHECKING_COUNT_ENEMIES = 8f;
    private const int MAX_ENEMIES = 5;

    private void Start()
    {
        FindTarget();
        StartCoroutine(SpawnEnemy());
    }

    private void FindTarget()
    {
        _target = FindObjectOfType<Base>().transform.position;
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(TIME_DELAY_SPAWNING);

        int type = Random.Range(0, _enemies.Length);
        int point = Random.Range(0, _spawnPoints.Length);

        GameObject enemy = Instantiate(_enemies[type].gameObject, _spawnPoints[point].transform.position, Quaternion.identity);
        Enemy enemyComponent = enemy.GetComponent<Enemy>();

        enemyComponent.OnDestroyed += RemoveDestroyedEnemyFromList;
        enemyComponent.SetBase(_target);

        _aliveEnemies.Add(enemy);

        if (_aliveEnemies.Count >= MAX_ENEMIES)
        {
            StartCoroutine(SpawnEnemy());
        }
        else
        {
            StartCoroutine(CheckIsCountEnemiesLowerMaximum());
        }

    }

    private IEnumerator CheckIsCountEnemiesLowerMaximum()
    {
        yield return new WaitForSeconds(TIME_DELAY_CHECKING_COUNT_ENEMIES);

        if (_aliveEnemies.Count < MAX_ENEMIES)
        {
            StartCoroutine(SpawnEnemy());
        }
        else
        {
            StartCoroutine(CheckIsCountEnemiesLowerMaximum());
        }
    }
    
    private void RemoveDestroyedEnemyFromList(GameObject destroyedObject)
    {
        _aliveEnemies.Remove(destroyedObject);
    }
}
