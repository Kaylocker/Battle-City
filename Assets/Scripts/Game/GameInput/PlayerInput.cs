using BattleCity.Game.Player;
using UnityEngine;

namespace BattleCity.Game.GameInput
{
    [RequireComponent(typeof(PlayerHealth), typeof(PlayerMovement), typeof(PlayerShooting))]
    public class PlayerInput : MonoBehaviour
    {
        private PlayerMovement _movement;
        private PlayerShooting _shooting;

        private void Awake()
        {
            TryGetComponent(out _movement);
            TryGetComponent(out _shooting);
        }

        private void Update()
        {
            Shooting();
            Movement();
            Rotation();
        }

        private void Shooting()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _shooting.isCanFire)
                _shooting.Shoot();
        }

        private void Movement()
        {
            if (Input.GetKey(KeyCode.W))
                _movement.SetMovementUp();
            else if (Input.GetKey(KeyCode.S))
                _movement.SetMovementDown();
            else
                _movement.SetMovementZero();
        }

        private void Rotation()
        {
            if (Input.GetKey(KeyCode.J))
                _movement.SetPositiveRotation();
            else if (Input.GetKey(KeyCode.K))
                _movement.SetNegativeRotation();

            _movement.Rotate();
        }
    }
}