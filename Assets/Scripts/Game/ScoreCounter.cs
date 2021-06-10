using UnityEngine;
using System;


public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private int _points;

    private static readonly EventAggregator<int> _onChangePoints = new EventAggregator<int>();
    public static void SubscribeChangePoints(Action<int> callback) => _onChangePoints.Subscribe(callback);
    public static void UnsubscribeChangePoints(Action<int> callback) => _onChangePoints.UnSubscribe(callback);

    private void OnEnable()
    {
        GameField.SubscribeDotDelete(AddPoints);
        GameState.SubscribeGameOver(Save);
        GameState.SubscribeExit(Save);
        PauseMenu.SubscribeRestart(OnRestart);
    }

    private void OnDisable()
    {
        GameField.UnsubscribeDotDelete(AddPoints);
        GameState.UnsubscribeGameOver(Save);
        GameState.UnsubscribeExit(Save);
        PauseMenu.UnsubscribeRestart(OnRestart);
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

        _onChangePoints.Publish(_points);
    }

    private void OnRestart()
    {
        _points = 0;

        _onChangePoints.Publish(_points);
    }
}