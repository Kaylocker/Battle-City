using UnityEngine;

namespace BattleCity.Game.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : PlayerBase
    {
        private float _speed;
        private float _rotationSpeed;

        private Rigidbody2D _rigidbody;
        private Vector2 _moveDirection;
        private float _angleRotationZ;

        private void Awake()
        {
            TryGetComponent(out _rigidbody);
            _speed = _settings.speed;
            _rotationSpeed = _settings.rotationSpeed;
        }

        private void FixedUpdate() => MovementLogic();

        public void SetMovementUp() => _moveDirection = transform.up;

        public void SetMovementDown() => _moveDirection = -transform.up;

        public void SetMovementZero() => _moveDirection = Vector2.zero;

        public void SetPositiveRotation() => _angleRotationZ += Time.deltaTime * _rotationSpeed;

        public void SetNegativeRotation() => _angleRotationZ -= Time.deltaTime * _rotationSpeed;

        public void Rotate() => transform.rotation = Quaternion.Euler(0, 0, _angleRotationZ);

        private void MovementLogic() => _rigidbody.velocity = _moveDirection * _speed * Time.deltaTime;
    }
}
