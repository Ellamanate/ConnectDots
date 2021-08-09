using System;
using UnityEngine;


public class TurnsCounter : MonoBehaviour
{
    public event Action<int> OnChangeAvailableTurns;

    [SerializeField] private PauseMenu _pause;
    [SerializeField] private GameField _field;
    [SerializeField] private int _startTurns;
    [SerializeField] private static int _availableTurns;

    public int AvailableTurns => _availableTurns;

    private void Awake()
    {
        _availableTurns = _startTurns;

        OnChangeAvailableTurns?.Invoke(_availableTurns);
    }

    private void OnEnable()
    {
        _field.OnEndTurn += OnEndTurn;
        _pause.OnRestart += OnRestart;
    }

    private void OnDisable()
    {
        _field.OnEndTurn -= OnEndTurn;
        _pause.OnRestart -= OnRestart;

        _availableTurns = 0;
    }

    private void OnEndTurn()
    {
        _availableTurns--;

        OnChangeAvailableTurns?.Invoke(_availableTurns);
    }

    private void OnRestart()
    {
        _availableTurns = _startTurns;

        OnChangeAvailableTurns?.Invoke(_availableTurns);
    }
}