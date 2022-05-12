using UnityEngine;

namespace BattleCity.Game.Player.Scriptables
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "BattleCity/PlayerSettings")]
    public class PlayerSettings : ScriptableObject
    {
        public float shootingReloadTime;
        public float speed;
        public float rotationSpeed;
        public int maxHitpoints;
    }
}