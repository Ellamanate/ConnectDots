using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;


public class GameField : MonoBehaviour
{
    public event Action<Dot> OnDeleteDot;
    public event Action OnEndTurn;

    [SerializeField] private Dot _dotPrefab;
    [SerializeField] private DotsConnecter _connecter;
    [SerializeField] private GameState _gameState;
    [SerializeField] private PauseMenu _pause;
    [SerializeField] private Palette _palette;
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _spawnLineDelay;
    private Dot[,] _dots;
    private Vector3 offset;

    private void Awake()
    {
        _dots = new Dot[_width, _height];

        offset = new Vector3((_width - 1) / 2f, (_height - 1) / 2f, -10);
        Camera.main.transform.position = offset;

        StartCoroutine(CreateDots());
    }

    private void OnEnable()
    {
        _connecter.OnLineRelease += DeleteDots;
        _connecter.OnSquareRelease += OnSquareRelease;
        _gameState.OnGameOver += OnGameOver;
        _pause.OnRestart += Restart;
    }

    private void OnDisable()
    {
        _connecter.OnLineRelease -= DeleteDots;
        _connecter.OnSquareRelease -= OnSquareRelease;
        _gameState.OnGameOver -= OnGameOver;
        _pause.OnRestart -= Restart;
    }

    private IEnumerator CreateDots()
    {
        for (int y = 0; y < _height; y++)
            yield return TryCreateLine(y);
    }

    private IEnumerator TryCreateLine(int y)
    {
        bool lineIsFull = true;

        for (int x = 0; x < _width; x++)
        {
            if (_dots[x, y] == null)
            {
                _dots[x, y] = Dot.Create(_dotPrefab, transform, x, y, _palette.Colors[Random.Range(0, _palette.Colors.Length)]);
                lineIsFull = false;
            }
        }

        if (!lineIsFull)
            yield return new WaitForSeconds(_spawnLineDelay);
    }

    private IEnumerator Grounding()
    {
        bool isGrounded = true;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 1; y < _height; y++)
            {
                if (_dots[x, y] != null)
                {
                    if (_dots[x, y - 1] == null)
                    {
                        _dots[x, y - 1] = _dots[x, y];
                        _dots[x, y].MoveToPoint(x, y - 1);
                        _dots[x, y] = null;

                        isGrounded = false;
                    }
                }
            }
        }

        yield return isGrounded ? CreateDots() : Grounding();
    }

    private void DeleteDots(Dot[] dots)
    {
        foreach (Dot dot in dots)
            DeleteDot(dot);

        StartCoroutine(Grounding());

        OnEndTurn?.Invoke();
    }

    private void OnSquareRelease(ColorInfo colorInfo)
    {
        foreach (Dot dot in _dots)
        {
            if (dot.ColorInfo.Id == colorInfo.Id)
                DeleteDot(dot);
        }

        StartCoroutine(Grounding());

        OnEndTurn?.Invoke();
    }

    private void DeleteDot(Dot dot)
    {
        _dots[dot.X, dot.Y] = null;

        OnDeleteDot?.Invoke(dot);

        Destroy(dot.gameObject);
    }

    private void ClearField()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (_dots[x, y] != null)
                {
                    Destroy(_dots[x, y].gameObject);
                    _dots[x, y] = null;
                }
            }
        }
    }

    private void OnGameOver()
    {
        StopAllCoroutines();

        ClearField();
    }

    private void Restart()
    {
        StartCoroutine(CreateDots());
    }
}