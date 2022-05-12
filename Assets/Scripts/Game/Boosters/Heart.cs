using UnityEngine;
using BattleCity.Game.Player;

namespace BattleCity.Game.Boosters
{
    public class Heart : Booster
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerHealth player))
            {
                player.TakeHealth(_settings.heartCount);
                DisableBooster(player);
            }
        }
    }
}