using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour,IDamagable
{
    public event Action<int> OnTakeDamage;
    public event Action<GameObject> OnDestroyed;

    [SerializeField] private GameObject _muzzle;
    [SerializeField] private GameObject _projectile;
    [SerializeField] protected float _speed;

    protected Vector3 _targetPosition;
    protected Vector3 _basePosition;
    protected Vector3 _playerPosition;
    protected Vector3[] _previousPositions = new Vector3[MOVEMENT_FRAMES];
    protected int _hitPoints = 1;
    private const int MOVEMENT_FRAMES = 3;
    private const float MINIMUM_STANDING_DISTANCE = 1f, RELOAD_TIME = 1f, SHOOTING_RATE = 3f, DELAY_BETWEEN_FINDING_PLAYER = 2f;
    private const float CORRECTION_ANGLE = 90f;
    private bool _isCanMove = true, _isCanFire = true;


    private void OnEnable()
    {
        OnTakeDamage += TakeDamage;

        SetStartPositions();

        StartCoroutine(ObstaclesAvailabilityController());
        StartCoroutine(Shooting());
        StartCoroutine(FindingPlayerTank());
    }

    private void OnDisable()
    {
        OnTakeDamage -= TakeDamage;
    }

    private void FixedUpdate()
    {
        if (_isCanMove)
            Movement();
    }

    private void SetStartPositions()
    {
        for (int i = 0; i < _previousPositions.Length; i++)
        {
            _previousPositions[i] = Vector3.zero;
        }
    }

    public void SetBase(Vector3 basePosition)
    {
        _basePosition = new Vector3(basePosition.x, basePosition.y, basePosition.z);
        SetCurrentTarget(_basePosition);
    }

    private void SetCurrentTarget(Vector3 target )
    {
        _targetPosition = target;
    }

    private void Movement()
    {
        Vector3 direction = _targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - CORRECTION_ANGLE, Vector3.forward);
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.fixedDeltaTime);
    }

    private IEnumerator ObstaclesAvailabilityController()
    {
        yield return new WaitForSeconds(3);

        _isCanMove = true;

        yield return new WaitForSeconds(1);

        SetPreviouslyPositionts();
        CheckIsCanMoving();

        StartCoroutine(ObstaclesAvailabilityController());
    }

    private void SetPreviouslyPositionts()
    {
        for (int i = 0; i < _previousPositions.Length - 1; i++)
        {
            _previousPositions[i] = _previousPositions[i + 1];
        }
        _previousPositions[_previousPositions.Length - 1] = transform.position;
    }

    private void CheckIsCanMoving()
    {
        for (int i = 0; i < _previousPositions.Length - 1; i++)
        {
            if (Vector3.Distance(_previousPositions[i], _previousPositions[i + 1]) >= MINIMUM_STANDING_DISTANCE)
            {
                _isCanMove = true;
                Shoot();
                break;
            }
            else
            {
                _isCanMove = false;
            }
        }
    }

    private IEnumerator Shooting()
    {
        yield return new WaitForSeconds(SHOOTING_RATE);

        Shoot();

        StartCoroutine(Shooting());
    }

    private void Shoot()
    {
        if (!_isCanFire)
        {
            return;
        }

        GameObject projectile = Instantiate(_projectile, _muzzle.transform.position, Quaternion.identity);
        projectile.transform.rotation = Quaternion.AngleAxis(transform.rotation.z, transform.forward);
        projectile.GetComponent<Projectile>().SetupDirection(transform.up);
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(RELOAD_TIME);
        _isCanFire = true;
    }

    public void TakeDamage(int damage)
    {
        _hitPoints -= damage;

        if (_hitPoints <= 0)
        {
            _hitPoints = 0;
            OnDestroyed?.Invoke(gameObject);
            Destroy(gameObject);
        }
    }

    private IEnumerator FindingPlayerTank()
    {
        yield return new WaitForSeconds(DELAY_BETWEEN_FINDING_PLAYER);

        bool isFindedPlayer = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f);

        foreach (var item in colliders)
        {
            if (item.TryGetComponent(out Player player))
            {
                isFindedPlayer = true;
                _playerPosition = player.gameObject.transform.position;
                print("finded");
                SetCurrentTarget(_playerPosition);
            }
        }

        if (!isFindedPlayer)
        {
            _targetPosition = _basePosition;
        }

        StartCoroutine(FindingPlayerTank());
    }
}
