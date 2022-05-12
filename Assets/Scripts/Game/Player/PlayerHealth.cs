using UnityEngine;
using System;
using System.Collections;

namespace BattleCity.Game.Player
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerHealth : PlayerBase, IDamagable
    {
        public event Action<int> OnChangeHitpoints;
        public event Action OnEndGame;

        [SerializeField] private AudioClip _takeDamageSound;

        private bool _isUnbreakable;
        private int _hitPoints;

        private void Awake()
        {
            _hitPoints = _settings.maxHitpoints;
        }

        public void TakeDamage(int damage)
        {
            if (_isUnbreakable) return;

            _hitPoints -= damage;
            _audioSource.PlayOneShot(_takeDamageSound);
            OnChangeHitpoints?.Invoke(_hitPoints);

            if (_hitPoints <= 0)
            {
                _hitPoints = 0;
                OnEndGame?.Invoke();
                Destroy(gameObject);
            }
        }

        public void TakeHealth(int hitPoints)
        {
            _hitPoints += hitPoints;

            if (_hitPoints >= _settings.maxHitpoints)
                _hitPoints = _settings.maxHitpoints;

            OnChangeHitpoints?.Invoke(_hitPoints);
        }

        public void MakePlayerUnbreakable(float time) => StartCoroutine(SetArmor(time));

        private IEnumerator SetArmor(float time)
        {
            _isUnbreakable = true;
            yield return new WaitForSeconds(time);
            _isUnbreakable = false;
        }
    }
}

