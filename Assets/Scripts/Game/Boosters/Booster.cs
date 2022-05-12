using UnityEngine;
using BattleCity.Game.Player.Interfaces;
using BattleCity.Game.Boosters.Scriptables;

namespace BattleCity.Game.Boosters
{
    public class Booster : MonoBehaviour
    {
        [SerializeField] protected AudioClip _boosterSound;
        [SerializeField] protected BoostersSettings _settings;

        protected void DisableBooster(ISound audioSource)
        {
            audioSource.PlaySound(_boosterSound);
            gameObject.SetActive(false);
        }
    }
}