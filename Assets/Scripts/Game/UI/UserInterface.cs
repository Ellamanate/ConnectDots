using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UserInterface : MonoBehaviour
{
    [SerializeField] private Button _menu;
    [SerializeField] private Text _score;
    [SerializeField] private PauseMenu _pauseMenu;

    private void Awake()
    {
        _menu.onClick.AddListener(ToMenu);
    }

    private void OnEnable()
    {
        Events.OnChangePoints.Subscribe(UpdateScore);
        Events.OnGameOver.AddListener(OnGameOver);
        Events.OnRestart.AddListener(OnRestart);
    }

    private void OnDisable()
    {
        Events.OnChangePoints.UnSubscribe(UpdateScore);
        Events.OnGameOver.RemoveListener(OnGameOver);
        Events.OnRestart.RemoveListener(OnRestart);
    }

    private void ToMenu()
    {
        Events.OnExit.Invoke();

        SceneManager.LoadScene("Menu");
    }

    private void UpdateScore(int score)
    {
        _score.text = "SCORE : " + score.ToString();
    }

    private void OnGameOver()
    {
        _pauseMenu.UpdateTitle("TURNS OVER");
        _pauseMenu.UpdateScore(_score.text, "BEST : " + Serialyzer.LoadBestScore().ToString());
        _pauseMenu.gameObject.SetActive(true);
    }

    private void OnRestart()
    {
        _pauseMenu.gameObject.SetActive(false);
    }
}