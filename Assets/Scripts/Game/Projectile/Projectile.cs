using UnityEngine;

namespace BattleCity.Game.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 150f;

        private Rigidbody2D _rigidBody;
        private Vector2 _direction;

        private void Start() => _rigidBody = GetComponent<Rigidbody2D>();

        private void FixedUpdate() => Movement();

        public void SetupDirection(Vector2 direction) => _direction = direction;

        public void IncreaseSpeed() => _speed += _speed;

        private void Movement() => _rigidBody.velocity = new Vector2(_direction.x * _speed * Time.fixedDeltaTime, _direction.y * _speed * Time.fixedDeltaTime);

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(1);
                gameObject.SetActive(false);
            }
        }
    }
}