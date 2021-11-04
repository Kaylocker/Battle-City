using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private Base _base;
    [SerializeField] private TimerUI _timer;
    [SerializeField] private ScoreUI _scoreUI;
    [SerializeField] private Text _summaryScore;

    private Player _player;

    private void OnEnable()
    {
        _player = FindObjectOfType<Player>();

        _player.OnEndGame += SetActiveGameOverMenu;
        _base.OnEndGame += SetActiveGameOverMenu;
        _timer.OnEndGame += SetActiveGameOverMenu;
    }

    private void OnDisable()
    {
        _player.OnEndGame -= SetActiveGameOverMenu;
        _base.OnEndGame -= SetActiveGameOverMenu;
        _timer.OnEndGame -= SetActiveGameOverMenu;
    }

    public void SetActiveGameOverMenu()
    {
        Time.timeScale = 0;
        _gameOverMenu.SetActive(true);
        _summaryScore.text = _scoreUI.ScoreCount.ToString();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
    }
}
