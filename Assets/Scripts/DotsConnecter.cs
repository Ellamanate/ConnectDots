using UnityEngine;
using Extensions;
using System.Collections.Generic;


public class DotsConnecter : MonoBehaviour
{
    [SerializeField] private LinePainter _line;
    [SerializeField] private GameField _gameField;
    [SerializeField] private List<Dot> _dots = new List<Dot>();
    [SerializeField] private Dot _lastDot;
    [SerializeField] private Dot _penultDot;
    private Camera _camera;
    private bool _clicked;
    [SerializeField] private bool _isSquare;
    [SerializeField] private Dot _squareCreated;

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
                _line.TryAddPoint(dot.transform.position, false);
                _dots.Add(dot);

                _penultDot = _lastDot;
                _lastDot = dot;

                Debug.Log("Add");
            }
            else if (_isSquare && dot != null && dot == _squareCreated)
            {
                _isSquare = false;
                _squareCreated = null;

                _line.RemoveLastPoint();
                _lastDot = _dots[_dots.Count - 1];

                if (_dots.Count > 1)
                    _penultDot = _dots[_dots.Count - 2];

                Debug.Log("DropSquare");
            }
            else if (_dots.Count > 1 && dot == _penultDot && _penultDot != _squareCreated)
            {
                _penultDot = _lastDot;

                _line.RemovePoint(_lastDot.transform.position);
                _dots.Remove(_lastDot);


                _lastDot = _dots[_dots.Count - 1];

                Debug.Log("Remove");
            }
            else if (!_isSquare && IsSquare(dot))
            {
                _line.TryAddPoint(dot.transform.position, true);

                _penultDot = _lastDot;
                _lastDot = dot;

                _isSquare = true;
                _squareCreated = _penultDot;
            }
        }

        _line.Draw();
    }

    private bool IsSquare(Dot dot)
    {
        return _dots.Count > 1 && _dots.Contains(dot) && dot != _penultDot && dot != _lastDot && CheckCoords(dot, _lastDot);
    }

    private void ReleaseSquare()
    {
        Debug.Log("ReleaseSquare");
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
        if (_dots.Count > 1)
            _gameField.DeleteDots(_dots.ToArray());

        _line.StopLine();
        _dots.Clear();
        _lastDot = null;

        _clicked = false;

        if (_isSquare)
            ReleaseSquare();
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