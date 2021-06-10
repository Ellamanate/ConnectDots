using UnityEngine;


public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private int _points;

    private void OnEnable()
    {
        Events.OnDeleteDot.Subscribe(AddPoints);
        Events.OnExit.AddListener(Save);
        Events.OnGameOver.AddListener(Save);
        Events.OnRestart.AddListener(OnRestart);
    }

    private void OnDisable()
    {
        Events.OnDeleteDot.UnSubscribe(AddPoints);
        Events.OnExit.RemoveListener(Save);
        Events.OnGameOver.RemoveListener(Save);
        Events.OnRestart.RemoveListener(OnRestart);
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

        Events.OnChangePoints.Publish(_points);
    }

    private void OnRestart()
    {
        _points = 0;

        Events.OnChangePoints.Publish(_points);
    }
}