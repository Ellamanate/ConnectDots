using System;
using UnityEngine;
using UnityEngine.UI;


public class UserInterface : MonoBehaviour
{
    public event Action OnMenuClick;

    [SerializeField] private Button _menu;
    [SerializeField] private Text _score;
    [SerializeField] private Text _turns;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private TurnsCounter _turnsCounter;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private GameState _gameState;

    private void Awake()
    {
        _menu.onClick.AddListener(() => OnMenuClick.Invoke());
        UpdateTurns(_turnsCounter.AvailableTurns);
        UpdateScore(_scoreCounter.Points);
    }

    private void OnEnable()
    {
        _turnsCounter.OnChangeAvailableTurns += UpdateTurns;
        _scoreCounter.OnChangePoints += UpdateScore;
        _gameState.OnGameOver += OnGameOver;
        _pauseMenu.OnRestart += OnRestart;
        Serialyzer.OnTryChangeBestScore += UpdateBestScore;
    }

    private void OnDisable()
    {
        _turnsCounter.OnChangeAvailableTurns -= UpdateTurns;
        _scoreCounter.OnChangePoints -= UpdateScore;
        _gameState.OnGameOver -= OnGameOver;
        _pauseMenu.OnRestart -= OnRestart;
        Serialyzer.OnTryChangeBestScore -= UpdateBestScore;
    }

    private void UpdateTurns(int turns)
    {
        _turns.text = "TURNS : " + turns.ToString();
    }

    private void UpdateScore(int score)
    {
        _score.text = "SCORE : " + score.ToString();
    }

    private void UpdateBestScore(int bestScore)
    {
        _pauseMenu.UpdateScore(_score.text, "BEST : " + bestScore.ToString());
    }

    private void OnGameOver()
    {
        _pauseMenu.UpdateTitle("NO MORE TURNS");
        _pauseMenu.gameObject.SetActive(true);
    }

    private void OnRestart()
    {
        _pauseMenu.gameObject.SetActive(false);
    }
}