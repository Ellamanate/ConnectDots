using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    [SerializeField] private Button _play;
    [SerializeField] private Text _bestScore;

    private void Awake()
    {
        _play.onClick.AddListener(Play);

        _bestScore.text = "BEST SCORE : " + Serialyzer.LoadBestScore().ToString();
    }

    private void Play()
    {
        SceneManager.LoadScene("Main");
    }
}