using System.Collections.Generic;
using UnityEngine;
using Extensions;


public class LinePainter : MonoBehaviour
{
    [SerializeField] private LineFactory _lineFactory;
    [SerializeField] private float _lineWidth;
    private List<Vector2> _points = new List<Vector2>();
    private Camera _camera;
    private Line drawnLine;
    private Color _color;

    public void Draw()
    {
        if (_points.Count > 0)
        {
            if (drawnLine != null)
                drawnLine.end = _camera.ScreenToWorldPoint(Input.mousePosition).ChangeZ(0);
        }
    }

    public void StartLine(Vector3 startPoint, Color color)
    {
        _color = color;

        _points.Add(startPoint);

        DrawLine(startPoint);
    }

    public void StopLine()
    {
        _points.Clear();
        _lineFactory.Clear();
    }

    public void TryAddPoint(Vector2 point, bool force)
    {
        if (force || !_points.Contains(point))
        {
            _points.Add(point);

            if (drawnLine != null)
                drawnLine.end = point;

            DrawLine(point);
        }
    }

    public void RemovePoint(Vector2 point)
    {
        if (_points.Contains(point))
        {
            _points.Remove(point);

            _lineFactory.DropLast();
            drawnLine = _lineFactory.GetLast();
        }
    }

    public void RemoveLastPoint()
    {
        if (_points.Count > 0)
            _points.RemoveAt(_points.Count - 1);

        _lineFactory.DropLast();
        drawnLine = _lineFactory.GetLast();
    }

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void DrawLine(Vector2 endPoint)
    {
        drawnLine = _lineFactory.GetLine(_points[_points.Count - 1], endPoint, _lineWidth, _color);
    }
}