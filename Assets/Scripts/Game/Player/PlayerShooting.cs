using System;
using System.Collections;
using UnityEngine;
using BattleCity.Game.Projectiles;

namespace BattleCity.Game.Player
{
    public class PlayerShooting : PlayerBase
    {
        public event Action OnShooted;

        [SerializeField] private GameObject _projectile;
        [SerializeField] private GameObject _muzzle;
        [SerializeField] private AudioClip _shootingSound;

        private float _shootingReloadTime;
        private bool _isCanFire = true;
        private bool _isIncreasedProjectileSpeed;

        public bool isCanFire => _isCanFire;

        private void Awake() => _shootingReloadTime = _settings.shootingReloadTime;

        private void Update() => ShootingInput();

        private void ShootingInput()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _isCanFire)
                Shoot();
        }

        public void StartIncreasingProjectileSpeed(float time) => StartCoroutine(IncreaseProjectileSpeed(time));

        public void Shoot()
        {
            GameObject projectile = Instantiate(_projectile, _muzzle.transform.position, Quaternion.identity);
            projectile.transform.localEulerAngles = transform.localEulerAngles;
            Projectile projectileComponent = projectile.GetComponent<Projectile>();
            projectileComponent.SetupDirection(transform.up);

            if (_isIncreasedProjectileSpeed)
                projectileComponent.IncreaseSpeed();

            _audioSource.PlayOneShot(_shootingSound);
            _isCanFire = false;
            OnShooted?.Invoke();

            StartCoroutine(Reload());
        }


        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(_shootingReloadTime);
            _isCanFire = true;
        }

        private IEnumerator IncreaseProjectileSpeed(float time)
        {
            _isIncreasedProjectileSpeed = true;
            yield return new WaitForSeconds(time);
            _isIncreasedProjectileSpeed = false;
        }
    }
}