using UnityEngine;
using Extensions;
using System.Collections.Generic;
using System;


public class DotsConnecter : MonoBehaviour
{
    public event Action<Dot[]> OnLineRelease;
    public event Action<ColorInfo> OnSquareRelease;

    [SerializeField] private LinePainter _line;
    private List<Dot> _dots = new List<Dot>();
    private Dot _lastDot;
    private Dot _squareCreated;
    private Camera _camera;
    private bool _clicked;
    private bool _isSquare;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && MouseOverDot(out Dot dot))
        {
            StartLine(dot);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ReleaseLine();
        }
    }

    private void OnGUI()
    {
        if (_clicked)
        {
            if (MouseOverDot(out Dot dot) && DotIsValid(dot))
            {
                AddDot(dot, false);
            }
            else if (_dots.Count > 1 && dot == _dots[_dots.Count - 2])
            {
                RemoveDot();

                if (_isSquare && dot != null && dot == _squareCreated)
                    RemoveSquare();
            }
            else if (!_isSquare && IsSquare(dot))
            {
                AddSquare();
                AddDot(dot, true);
            }
        }

        _line.Draw();
    }

    private void AddDot(Dot dot, bool force)
    {
        _line.TryAddPoint(dot.transform.position, force);
        _dots.Add(dot);

        _lastDot = dot;
    }

    private void AddSquare()
    {
        _squareCreated = _dots[_dots.Count - 1];
        _isSquare = true;
    }

    private void RemoveDot()
    {
        _line.RemoveLastPoint();
        _dots.RemoveAt(_dots.Count - 1);

        _lastDot = _dots[_dots.Count - 1];
    }

    private void RemoveSquare()
    {
        _isSquare = false;
        _squareCreated = null;
    }

    private bool IsSquare(Dot dot)
    {
        return _dots.Count > 1 && _dots.Contains(dot) && dot != _dots[_dots.Count - 1] && dot != _lastDot && CheckCoords(dot, _lastDot);
    }

    private void StartLine(Dot dot)
    {
        _clicked = true;

        _line.StartLine(dot.transform.position, dot.ColorInfo.Color);

        _dots.Add(dot);
        _lastDot = dot;
    }

    private void ReleaseLine()
    {
        if (_isSquare)
        {
            OnSquareRelease?.Invoke(_dots[0].ColorInfo);
        }
        else if(_dots.Count > 1)
        {
            OnLineRelease?.Invoke(_dots.ToArray());
        }

        _line.StopLine();
        _dots.Clear();

        _clicked = false;
        _isSquare = false;
        _squareCreated = null;
        _lastDot = null;
    }

    private bool MouseOverDot(out Dot dot)
    {
        RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition).ChangeZ(0), Vector2.zero);

        if (hit.collider != null && hit.collider.TryGetComponent(out dot))
            return true;

        dot = null;

        return false;
    }

    private bool DotIsValid(Dot dot)
    {
        return (_dots.Count == 0) || (dot.ColorInfo.Id == _lastDot.ColorInfo.Id && !_dots.Contains(dot) && CheckCoords(dot, _lastDot));
    }

    private bool CheckCoords(Dot first, Dot second)
    {
        return first.X == second.X && Mathf.Abs(first.Y - second.Y) == 1 || 
               first.Y == second.Y && Mathf.Abs(first.X - second.X) == 1;
    }
}