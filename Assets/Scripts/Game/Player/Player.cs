using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _shootingReloadTime;

    private Rigidbody2D _rigidBody;
    private Vector2 _moveDirection;
    private float _angleRotationZ;
    private bool _isCanFire = true;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
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
        GameObject projectile = Instantiate(_projectile, transform.position, Quaternion.identity);
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
}
