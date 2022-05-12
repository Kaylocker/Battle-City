using UnityEngine;
using BattleCity.Game.Player;

namespace BattleCity.Game.Boosters
{
    public class Shield : Booster
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerHealth player))
            {
                player.MakePlayerUnbreakable(_settings.shieldTime);
                DisableBooster(player);
            }
        }
    }
}