using System.Collections;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Dot : MonoBehaviour
{
    [SerializeField] private int _speed;
    private SpriteRenderer _renderer;
    private Coroutine _move;
    private ColorInfo _colorInfo;
    private int _x;
    private int _y;

    public int X => _x;
    public int Y => _y;
    public ColorInfo ColorInfo => _colorInfo;

    public static Dot Create(Dot dot, Transform parent, int x, int y, ColorInfo color)
    {
        Dot created = Instantiate(dot, new Vector2(x, y) + new Vector2(0, 5), Quaternion.identity, parent);

        created._colorInfo = color;
        created._renderer.color = color.Color;
        created.MoveToPoint(x, y);

        return created;
    }

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void MoveToPoint(int x, int y)
    {
        _x = x;
        _y = y;

        if (_move != null)
            StopCoroutine(_move);

        _move = StartCoroutine(Move(new Vector2(x, y)));
    }

    private IEnumerator Move(Vector3 endPosition)
    {
        Vector3 startPosition = transform.position;
        float time = 0;
        float distance = Vector3.Distance(startPosition, endPosition);
        float speed = Mathf.Abs(startPosition.y - endPosition.y) * _speed;

        while (time < 1) 
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, time);
            time += Time.deltaTime * speed / distance;

            yield return new WaitForEndOfFrame();
        }

        transform.position = endPosition;
    }
}