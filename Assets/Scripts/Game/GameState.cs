using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameState : MonoBehaviour
{
    private static readonly UnityEvent _onGameOver = new UnityEvent();
    private static readonly UnityEvent _onExit = new UnityEvent();

    public static void SubscribeGameOver(UnityAction callback) => _onGameOver.AddListener(callback);
    public static void UnsubscribeGameOver(UnityAction callback) => _onGameOver.AddListener(callback);
    public static void SubscribeExit(UnityAction callback) => _onExit.AddListener(callback);
    public static void UnsubscribeExit(UnityAction callback) => _onExit.AddListener(callback);

    private void OnEnable()
    {
        UserInterface.SubscribeMenuClick(ToMenu);
        TurnsCounter.SubscribeChangeAvailableTurns(CheckTurns);
    }

    private void OnDisable()
    {
        UserInterface.UnsubscribeMenuClick(ToMenu);
        TurnsCounter.UnsubscribeChangeAvailableTurns(CheckTurns);
    }

    private void CheckTurns(int turns)
    {
        if (turns <= 0)
            _onGameOver.Invoke();
    }

    private void ToMenu()
    {
        _onExit.Invoke();

        SceneManager.LoadScene("Menu");
    }
}