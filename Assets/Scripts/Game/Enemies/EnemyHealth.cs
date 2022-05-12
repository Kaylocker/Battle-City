using UnityEngine;
using System;
using Random = UnityEngine.Random;
using BattleCity.Game.Boosters;

namespace BattleCity.Game.Enemy
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]

    public class EnemyHealth : MonoBehaviour, IDamagable
    {
        public event Action<GameObject> OnDestroyed;
        public event Action<int> OnScoreChanged;

        [SerializeField] private AudioClip _takeDamageSound;

        private AudioSource _audioSource;
        private ScoreUI _score;
        protected Booster _booster;
        protected int _hitPoints = 1;
        protected int _scoreForKill;

        private const float _CHANCE_DROP_BOOSTER = 0.8f;

        private void Awake() => TryGetComponent(out _audioSource);

        private void OnEnable()
        {
            _score = FindObjectOfType<ScoreUI>();
            OnScoreChanged += _score.SetScore;
            OnDestroyed += DropBooster;
        }

        private void OnDisable()
        {
            OnScoreChanged -= _score.SetScore;
            OnDestroyed -= DropBooster;
        }

        public void TakeDamage(int damage)
        {
            _hitPoints -= damage;

            if (_hitPoints <= 0)
            {
                _audioSource.PlayOneShot(_takeDamageSound);
                OnScoreChanged?.Invoke(_scoreForKill);
                OnDestroyed?.Invoke(gameObject);
                gameObject.SetActive(false);
            }
        }

        private void DropBooster(GameObject gameObject)
        {
            float random = Random.Range(0f, 1f);

            if (random >= _CHANCE_DROP_BOOSTER)
            {
                Instantiate(_booster.gameObject, transform.position, Quaternion.identity);
            }
        }
    }
}