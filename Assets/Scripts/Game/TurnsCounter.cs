using System;
using UnityEngine;


public class TurnsCounter : MonoBehaviour
{
    [SerializeField] private int _startTurns;
    [SerializeField] private static int _availableTurns;

    private static EventAggregator<int> _onChangeAvailableTurns = new EventAggregator<int>();

    public static void SubscribeChangeAvailableTurns(Action<int> callback) => _onChangeAvailableTurns.Subscribe(callback);
    public static void UnsubscribeChangeAvailableTurns(Action<int> callback) => _onChangeAvailableTurns.UnSubscribe(callback);
    public static int AvailableTurns => _availableTurns;

    private void Awake()
    {
        _availableTurns = _startTurns;

        _onChangeAvailableTurns.Publish(_availableTurns);
    }

    private void OnEnable()
    {
        GameField.SubscribeEndTurn(OnEndTurn);
        PauseMenu.SubscribeRestart(OnRestart);
    }

    private void OnDisable()
    {
        GameField.UnsubscribeEndTurn(OnEndTurn);
        PauseMenu.UnsubscribeRestart(OnRestart);

        _availableTurns = 0;
    }

    private void OnEndTurn()
    {
        _availableTurns--;

        _onChangeAvailableTurns.Publish(_availableTurns);
    }

    private void OnRestart()
    {
        _availableTurns = _startTurns;

        _onChangeAvailableTurns.Publish(_availableTurns);
    }
}