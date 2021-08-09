using UnityEngine;
using System;


public class ScoreCounter : MonoBehaviour
{
    public event Action<int> OnChangePoints;

    [SerializeField] private PauseMenu _pause;
    [SerializeField] private GameField _field;
    [SerializeField] private GameState _gameState;
    [SerializeField] private static int _points;

    public int Points => _points;

    private void Awake()
    {
        OnChangePoints?.Invoke(_points);
    }

    private void OnEnable()
    {
        _field.OnDeleteDot += AddPoints;
        _gameState.OnGameOver += Save;
        _gameState.OnExit += Save;
        _pause.OnRestart += OnRestart;
    }

    private void OnDisable()
    {
        _field.OnDeleteDot -= AddPoints;
        _gameState.OnGameOver -= Save;
        _gameState.OnExit -= Save;
        _pause.OnRestart -= OnRestart;

        _points = 0;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            Save();
    }

    private void Save() => Serialyzer.TrySaveScore(_points);

    private void AddPoints(Dot dot)
    {
        _points++;

        OnChangePoints?.Invoke(_points);
    }

    private void OnRestart()
    {
        _points = 0;

        OnChangePoints?.Invoke(_points);
    }
}