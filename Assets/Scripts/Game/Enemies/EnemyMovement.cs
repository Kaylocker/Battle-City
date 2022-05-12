using BattleCity.Game.Player;
using BattleCity.Game.Enemy.Types.Scriptable;
using System.Collections;
using UnityEngine;

namespace BattleCity.Game.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private TanksTypeScriptable _tankSettings;

        private GameObject _target;
        private GameObject _base;
        protected GameObject _player;
        private bool _isCanMove = true;
        private Vector3[] _previousPositions = new Vector3[_MOVEMENT_FRAMES];
        private float _speed;

        private const int _MOVEMENT_FRAMES = 3;
        private const float _CORRECTION_ANGLE = 90f;
        private const float _MINIMUM_STANDING_DISTANCE = 1f;
        private const float _DELAY_BETWEEN_FINDING_PLAYER = 0.1f;
        private const float _RADIUS_DETECT_PLAYER = 5f;

        private void Awake()
        {
            _speed = _tankSettings.speed;
            SetStartPositions();
        }

        private void OnEnable()
        {
            StartCoroutine(FindingPlayerTank());
            StartCoroutine(ObstaclesAvailabilityController());
        }

        private void FixedUpdate()
        {
            if (_isCanMove && _target != null)
                Movement();
        }

        public void SetBase(GameObject basePosition)
        {
            _base = basePosition;
            SetCurrentTarget(_base);
        }

        public void SetPlayerTarget(GameObject player) => _player = player;

        private void SetCurrentTarget(GameObject target) => _target = target;

        private void SetStartPositions()
        {
            for (int i = 0; i < _previousPositions.Length; i++)
            {
                _previousPositions[i] = Vector3.zero;
            }
        }

        private void Movement()
        {
            Vector3 position = transform.position;
            Vector3 targetPosition = _target.transform.position;
            Vector3 direction = targetPosition - position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - _CORRECTION_ANGLE, Vector3.forward);
            transform.position = Vector3.MoveTowards(position, targetPosition, _speed * Time.fixedDeltaTime);
        }

        private void SetPreviouslyPositions()
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
                if (Vector3.Distance(_previousPositions[i], _previousPositions[i + 1]) >= _MINIMUM_STANDING_DISTANCE)
                {
                    _isCanMove = true;
                    break;
                }
                else
                    _isCanMove = false;
            }
        }

        private IEnumerator ObstaclesAvailabilityController()
        {
            while (gameObject.activeInHierarchy)
            {
                _isCanMove = true;
                yield return new WaitForSeconds(1f);

                SetPreviouslyPositions();
                CheckIsCanMoving();
            }
        }

        private IEnumerator FindingPlayerTank()
        {
            while (gameObject.activeInHierarchy)
            {
                yield return new WaitForSeconds(_DELAY_BETWEEN_FINDING_PLAYER);

                bool isFindedPlayer = false;
                if (Vector3.Distance(gameObject.transform.position, _player.transform.position) <= _RADIUS_DETECT_PLAYER)
                {
                    isFindedPlayer = true;
                    SetCurrentTarget(_player);
                }

                if (!isFindedPlayer)
                    _target = _base;
            }
        }
    }
}