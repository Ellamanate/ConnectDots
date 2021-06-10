using System.Collections;
using UnityEngine;


public class GameField : MonoBehaviour
{
    [SerializeField] private Dot _dot;
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
        Events.OnLineRelease.Subscribe(DeleteDots);
        Events.OnSquareRelease.Subscribe(OnSquareRelease);
        Events.OnGameOver.AddListener(OnGameOver);
        Events.OnRestart.AddListener(OnRestart);
    }

    private void OnDisable()
    {
        Events.OnLineRelease.UnSubscribe(DeleteDots);
        Events.OnSquareRelease.UnSubscribe(OnSquareRelease);
        Events.OnGameOver.RemoveListener(OnGameOver);
        Events.OnRestart.RemoveListener(OnRestart);
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
                _dots[x, y] = Dot.Create(_dot, transform, x, y, _palette.Colors[Random.Range(0, _palette.Colors.Length)]);
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

        Events.OnEndTurn.Invoke();
    }

    private void OnSquareRelease(ColorInfo colorInfo)
    {
        foreach (Dot dot in _dots)
        {
            if (dot.ColorInfo.Id == colorInfo.Id)
                DeleteDot(dot);
        }

        StartCoroutine(Grounding());

        Events.OnEndTurn.Invoke();
    }

    private void DeleteDot(Dot dot)
    {
        _dots[dot.X, dot.Y] = null;

        Events.OnDeleteDot.Publish(dot);

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

    private void OnRestart()
    {
        StartCoroutine(CreateDots());
    }
}