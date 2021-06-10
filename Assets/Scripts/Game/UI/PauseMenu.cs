using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Text _title;
    [SerializeField] private Text _score;
    [SerializeField] private Text _bestScore;
    [SerializeField] private Button _restart;

    private static readonly UnityEvent _onRestart = new UnityEvent();

    public static void SubscribeRestart(UnityAction callback) => _onRestart.AddListener(callback);
    public static void UnsubscribeRestart(UnityAction callback) => _onRestart.AddListener(callback);

    public void UpdateTitle(string title)
    {
        _title.text = title;
    }

    public void UpdateScore(string score, string bestScore)
    {
        _score.text = score;
        _bestScore.text = bestScore;
    }

    private void Awake()
    {
        _restart.onClick.AddListener(_onRestart.Invoke);
    }
}