using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private Text _score;
    public int ScoreCount { get; private set; }

    private void Start()
    {
        SetScore(ScoreCount);
    }

    public void SetScore(int score)
    {
        ScoreCount += score;
        _score.text = "SCORE:" + ScoreCount.ToString();
    }
}
