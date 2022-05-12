using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using BattleCity.Game.Player;

namespace BattleCity.Game.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        public event Action OnCreatedTank;
        public event Action OnDestroyedTank;

        [SerializeField] private GameObject[] _enemies;
        [SerializeField] private GameObject[] _spawnPoints;

        private EnemiesCountUI _enemiesUI;
        private GameObject _target;
        private GameObject _player;
        private List<GameObject> _aliveEnemies = new List<GameObject>();
        private const float TIME_DELAY_SPAWNING = 2f, TIME_DELAY_CHECKING_COUNT_ENEMIES = 2f;
        private const int MAX_ENEMIES = 6;

        private void Start()
        {
            _target = FindObjectOfType<Base>().gameObject;
            _player = FindObjectOfType<PlayerHealth>().gameObject;
            StartCoroutine(SpawnEnemy());
        }

        private void OnEnable()
        {
            _enemiesUI = FindObjectOfType<EnemiesCountUI>();

            OnCreatedTank += _enemiesUI.SetActiveEnemy;
            OnDestroyedTank += _enemiesUI.DeleteActiveEnemy;
        }

        private void OnDisable()
        {
            OnCreatedTank -= _enemiesUI.SetActiveEnemy;
            OnDestroyedTank -= _enemiesUI.DeleteActiveEnemy;
        }

        private void FindTarget()
        {
            _target = FindObjectOfType<Base>().gameObject;
        }

        private IEnumerator SpawnEnemy()
        {
            yield return new WaitForSeconds(TIME_DELAY_SPAWNING);

            int type = Random.Range(0, _enemies.Length);
            int point = Random.Range(0, _spawnPoints.Length);

            GameObject enemy = Instantiate(_enemies[type].gameObject, _spawnPoints[point].transform.position, Quaternion.identity);
            enemy.TryGetComponent(out EnemyMovement enemyMovement);
            enemyMovement.SetBase(_target);
            enemyMovement.SetPlayerTarget(_player);
            EnemyHealth enemyComponent = enemy.GetComponent<EnemyHealth>();
            enemyComponent.OnDestroyed += RemoveDestroyedEnemyFromList;

            _aliveEnemies.Add(enemy);
            OnCreatedTank?.Invoke();

            if (_aliveEnemies.Count > MAX_ENEMIES)
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
            OnDestroyedTank?.Invoke();
        }
    }
}