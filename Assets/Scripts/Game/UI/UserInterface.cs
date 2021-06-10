using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UserInterface : MonoBehaviour
{
    [SerializeField] private Button _menu;
    [SerializeField] private Text _score;
    [SerializeField] private Text _turns;
    [SerializeField] private PauseMenu _pauseMenu;

    private static readonly UnityEvent _onMenuClick = new UnityEvent();

    public static void SubscribeMenuClick(UnityAction callback) => _onMenuClick.AddListener(callback);
    public static void UnsubscribeMenuClick(UnityAction callback) => _onMenuClick.AddListener(callback);


    private void Awake()
    {
        _menu.onClick.AddListener(_onMenuClick.Invoke);
    }

    private void OnEnable()
    {
        ScoreCounter.SubscribeChangePoints(UpdateScore);
        Serialyzer.SubscribeTryChangeBestScore(UpdateBestScore);
        TurnsCounter.SubscribeChangeAvailableTurns(UpdateTurns);
        GameState.SubscribeGameOver(OnGameOver);
        PauseMenu.SubscribeRestart(OnRestart);
    }

    private void OnDisable()
    {
        ScoreCounter.UnsubscribeChangePoints(UpdateScore);
        Serialyzer.UnsubscribeTryChangeBestScore(UpdateBestScore);
        TurnsCounter.UnsubscribeChangeAvailableTurns(UpdateTurns);
        GameState.UnsubscribeGameOver(OnGameOver);
        PauseMenu.UnsubscribeRestart(OnRestart);
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
        _pauseMenu.UpdateTitle("TURNS OVER");
        _pauseMenu.gameObject.SetActive(true);
    }

    private void OnRestart()
    {
        _pauseMenu.gameObject.SetActive(false);
    }
}