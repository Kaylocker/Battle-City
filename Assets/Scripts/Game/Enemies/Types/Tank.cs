using UnityEngine;
using BattleCity.Game.Enemy.Types.Scriptable;

namespace BattleCity.Game.Enemy.Types
{
    public class Tank : EnemyHealth
    {
        [SerializeField] private TanksTypeScriptable _tankSettings;

        private void Start()
        {
            _scoreForKill = _tankSettings.scoreForKill;
            _hitPoints = _tankSettings.hitpoints;
            _booster = _tankSettings.booster;
        }
    }
}