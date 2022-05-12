using UnityEngine;
using BattleCity.Game.Player.Interfaces;
using BattleCity.Game.Player.Scriptables;

namespace BattleCity.Game.Player
{
    public class PlayerBase : MonoBehaviour, ISound
    {
        [SerializeField] protected PlayerSettings _settings;

        protected AudioSource _audioSource;

        private void OnEnable() => TryGetComponent(out _audioSource);

        public void PlaySound(AudioClip sound) => _audioSource.PlayOneShot(sound);
    }
}