using UnityEngine;
using BattleCity.Game.Boosters;

namespace BattleCity.Game.Enemy.Types.Scriptable
{
    [CreateAssetMenu(fileName = "TankType", menuName = "BattleCity/TankType")]
    public class TanksTypeScriptable : ScriptableObject
    {
        public Booster booster;
        [Range(0.1f, 10f)]
        public float speed;
        [Range(1,10)]
        public int hitpoints;
        [Range(10, 500)]
        public int scoreForKill;
        [Range(1f, 10f)]
        public float reloadTime;
    }
}