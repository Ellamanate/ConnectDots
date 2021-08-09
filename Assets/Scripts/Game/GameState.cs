using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameState : MonoBehaviour
{
    public event Action OnGameOver;
    public event Action OnExit;

    [SerializeField] private UserInterface _userInterface;
    [SerializeField] private TurnsCounter _turnsCounter;

    private void OnEnable()
    {
        _userInterface.OnMenuClick += ToMenu;
        _turnsCounter.OnChangeAvailableTurns += CheckTurns;
    }

    private void OnDisable()
    {
        _userInterface.OnMenuClick -= ToMenu;
        _turnsCounter.OnChangeAvailableTurns -= CheckTurns;
    }

    private void CheckTurns(int turns)
    {
        if (turns <= 0)
            OnGameOver?.Invoke();
    }

    private void ToMenu()
    {
        OnExit?.Invoke();

        SceneManager.LoadScene("Menu");
    }
}