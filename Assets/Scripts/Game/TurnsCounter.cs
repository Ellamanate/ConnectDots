using System.Collections;
using UnityEngine;


public class TurnsCounter : MonoBehaviour
{
    [SerializeField] private int _startTurns;
    private int _availableTurns;

    private void Awake()
    {
        _availableTurns = _startTurns;
    }

    private void OnEnable()
    {
        Events.OnEndTurn.AddListener(OnEndTurn);
        Events.OnRestart.AddListener(OnRestart);
    }

    private void OnDisable()
    {
        Events.OnEndTurn.RemoveListener(OnEndTurn);
        Events.OnRestart.RemoveListener(OnRestart);
    }

    private void OnEndTurn()
    {
        _availableTurns--;

        if (_availableTurns <= 0)
            GameOver();
    }

    private void GameOver()
    {
        Events.OnGameOver.Invoke();
    }

    private void OnRestart()
    {
        _availableTurns = _startTurns;
    }
}