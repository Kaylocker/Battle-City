using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Player : MonoBehaviour, IDamagable
{
    public event Action<int> OnChangeHitpoints;
    public event Action OnEndGame;
    public event Action OnShooted;

    [SerializeField] private AudioClip _shootingSound;
    [SerializeField] private AudioClip _takeDamageSound;
    [SerializeField] private GameObject _muzzle;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _shootingReloadTime;

    private AudioSource _audioSource;
    private Rigidbody2D _rigidBody;
    private Vector2 _moveDirection;
    private float _angleRotationZ;
    private bool _isCanFire = true, _isIncreasedProjectileSpeed, _isUnbreakable;
    private int _hitPoints = MAX_HITPOINTS;
    private const int MAX_HITPOINTS = 3;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        MovementInput();
        ShootingInput();
        Rotation();
    }

    private void FixedUpdate()
    {
        MovementLogic();
    }

    private void MovementInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _moveDirection = transform.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _moveDirection = -transform.up;
        }
        else
        {
            _moveDirection = Vector2.zero;
        }
    }

    private void MovementLogic()
    {
        _rigidBody.velocity = _moveDirection * _speed * Time.deltaTime;
    }

    private void Rotation()
    {
        if (Input.GetKey(KeyCode.J))
        {
            _angleRotationZ += Time.deltaTime * _rotationSpeed;
        }
        else if (Input.GetKey(KeyCode.K))
        {
            _angleRotationZ -= Time.deltaTime * _rotationSpeed;
        }

        transform.rotation = Quaternion.Euler(0, 0, _angleRotationZ);
    }

    private void ShootingInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isCanFire)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(_projectile, _muzzle.transform.position, Quaternion.identity);
        projectile.transform.rotation = Quaternion.AngleAxis(_angleRotationZ, transform.forward);
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        projectileComponent.SetupDirection(transform.up);

        if (_isIncreasedProjectileSpeed)
        {
            projectileComponent.IncreaseSpeed();
        }

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

    public void TakeDamage(int damage)
    {
        if (_isUnbreakable)
        {
            return;
        }

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

    public void TakeHealth()
    {
        _hitPoints++;

        if (_hitPoints >= MAX_HITPOINTS)
        {
            _hitPoints = MAX_HITPOINTS;
        }

        OnChangeHitpoints?.Invoke(_hitPoints);
    }


    //TOOD: Refactor
    public void StartIncreasingProjectileSpeed()
    {
        StartCoroutine(IncreaseProjectileSpeed());
    }

    private IEnumerator IncreaseProjectileSpeed()
    {
        _isIncreasedProjectileSpeed = true;
        float boostedTime = 10f;

        yield return new WaitForSeconds(boostedTime);
        _isIncreasedProjectileSpeed = false;
    }

    public void MakePlayerUnbreakable()
    {
        StartCoroutine(SetArmor());
    }

    private IEnumerator SetArmor()
    {
        _isUnbreakable = true;
        float boostedTime = 10f;

        yield return new WaitForSeconds(boostedTime);
        _isUnbreakable = false;
    }
}
