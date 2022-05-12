using UnityEngine;
using BattleCity.Game.Player;

namespace BattleCity.Game.Boosters
{
    public class ShootingBooster : Booster
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerShooting player))
            {
                player.StartIncreasingProjectileSpeed(_settings.shootIncreasingTime);
                DisableBooster(player);
            }
        }
    }
}