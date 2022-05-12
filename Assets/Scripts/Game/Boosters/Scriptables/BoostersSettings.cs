using UnityEngine;

namespace BattleCity.Game.Boosters.Scriptables
{
    [CreateAssetMenu(fileName = "BoostersSettings", menuName = "BattleCity/BoostersSettings")]
    public class BoostersSettings : ScriptableObject
    {
        public float shootIncreasingTime;
        public float shieldTime;
        public int heartCount;
    }
}