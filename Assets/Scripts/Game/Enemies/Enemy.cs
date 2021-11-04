using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] protected float _speed;
    private Rigidbody2D _rigidBody;

    protected Vector3 _target;
    protected Vector3 [] _previousPositions = new Vector3[MOVEMENT_FRAMES];
    protected int _hitPoints = 1;
    private const int MOVEMENT_FRAMES = 3;
    private const float MINIMUM_STANDING_DISTANCE = 1f, RELOAD_TIME = 1f, SHOOTING_RATE = 7f;
    private bool _isCanMove = true, _isCanFire = true;


    private void OnEnable()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

        for (int i = 0; i < _previousPositions.Length; i++)
        {
            _previousPositions[i] = Vector3.zero;
        }

        StartCoroutine(ObstaclesAvailabilityController());
        StartCoroutine(Shooting());
    }

    private void FixedUpdate()
    {
        if(_isCanMove)
        Movement();
    }

    public void SetTarget(Vector3 target)
    {
        _target = target;
    }
    
    private void Movement()
    {
        float correctionAngle = 90;

        Vector3 direction = _target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle- correctionAngle, Vector3.forward);

        transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.fixedDeltaTime);
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

        GameObject projectile = Instantiate(_projectile, transform.position, Quaternion.identity);
        projectile.transform.rotation = Quaternion.AngleAxis(transform.rotation.z, transform.forward);
        projectile.GetComponent<Projectile>().SetupDirection(transform.up);
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(RELOAD_TIME);
        _isCanFire = true;
    }
}
