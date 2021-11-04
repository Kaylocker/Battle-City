using System;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public event Action OnEndGame;

    [SerializeField] private Text _timer;
    private float _timeToWin;

    private const float TIME_TO_WIN = 150f;

    private void Start()
    {
        _timeToWin = TIME_TO_WIN;
    }

    private void Update()
    {
        if (_timeToWin <= 0)
        {
            _timer.text = "WIN WIN WIN";
            return;
        }

        _timeToWin -= Time.deltaTime;
        _timer.text = Mathf.Round(_timeToWin).ToString() + " seconds";
    }
}
