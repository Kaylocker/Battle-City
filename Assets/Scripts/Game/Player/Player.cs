using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Player : MonoBehaviour, IDamagable
{
    public event Action<int> OnTakeDamage;
    public event Action OnGameOver;

    [SerializeField] private GameObject _muzzle;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _shootingReloadTime;

    private Rigidbody2D _rigidBody;
    private Vector2 _moveDirection;
    private float _angleRotationZ;
    private bool _isCanFire = true;
    private int _hitPoints = 3;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        OnTakeDamage += TakeDamage;
    }

    private void OnDisable()
    {
        OnTakeDamage -= TakeDamage;
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
        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        Vector3 projectilePosition = new Vector3(collider.bounds.extents.x, collider.bounds.extents.y, 0);
        GameObject projectile = Instantiate(_projectile, _muzzle.transform.position, Quaternion.identity);
        projectile.transform.rotation = Quaternion.AngleAxis(_angleRotationZ, transform.forward);
        projectile.GetComponent<Projectile>().SetupDirection(transform.up);
        _isCanFire = false;
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(_shootingReloadTime);
        _isCanFire = true;
    }

    public void TakeDamage(int damage)
    {
        _hitPoints -= damage;

        if (_hitPoints <= 0)
        {
            _hitPoints = 0;
            Destroy(gameObject);
        }
    }
}
