using System.Collections;
using UnityEngine;
using BattleCity.Game.Projectiles;
using BattleCity.Game.Enemy.Types.Scriptable;

namespace BattleCity.Game.Enemy
{
    public class EnemyShooting : MonoBehaviour
    {
        [SerializeField] TanksTypeScriptable _tankSettings;
        [SerializeField] private GameObject _projectile;
        [SerializeField] private GameObject _muzzle;
        [SerializeField] private AudioClip _shootingSound;

        private AudioSource _audioSource;
        private bool _isCanFire = true;
        private float _reloadTime;

        private const float SHOOTING_RATE = 0.5f;

        private void Awake()
        {
            TryGetComponent(out _audioSource);
            _reloadTime = _tankSettings.reloadTime;
        }

        private void OnEnable() => StartCoroutine(Shooting());

        private IEnumerator Shooting()
        {
            while (gameObject.activeInHierarchy)
            {
                yield return new WaitForSeconds(SHOOTING_RATE);
                Shoot();
            }
        }

        public void Shoot()
        {
            if (!_isCanFire)
                return;

            _isCanFire = false;

            GameObject projectile = Instantiate(_projectile, _muzzle.transform.position, Quaternion.identity);
            projectile.transform.rotation = Quaternion.AngleAxis(transform.rotation.z, transform.forward);
            projectile.GetComponent<Projectile>().SetupDirection(transform.up);
            _audioSource.PlayOneShot(_shootingSound);
            StartCoroutine(Reload());
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(_reloadTime);
            _isCanFire = true;
        }
    }
}