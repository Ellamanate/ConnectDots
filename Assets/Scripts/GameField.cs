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

    public void DeleteDots(Dot[] dots)
    {
        foreach (Dot dot in dots)
        {
            _dots[dot.X, dot.Y] = null;

            Destroy(dot.gameObject);
        }

        StartCoroutine(Grounding());
    }

    public void Square(ColorInfo colorInfo)
    {
        foreach (Dot dot in _dots)
        {
            if (dot.ColorInfo.Id == colorInfo.Id)
            {
                _dots[dot.X, dot.Y] = null;

                Destroy(dot.gameObject);
            }
        }

        StartCoroutine(Grounding());
    }

    private void Awake()
    {
        _dots = new Dot[_width, _height];

        offset = new Vector3((_width - 1) / 2f, (_height - 1) / 2f, -10);
        Camera.main.transform.position = offset;

        StartCoroutine(CreateDots());
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
}
