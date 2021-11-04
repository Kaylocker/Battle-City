using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private Text _score;
    private int _scoreCount;

    public void SetScore(int score)
    {
        _scoreCount += score;
        _score.text = "SCORE:" + _scoreCount.ToString();
    }
}
