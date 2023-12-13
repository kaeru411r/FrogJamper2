using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Xml;
using UniRx.Triggers;

/// <summary>
/// フィールド
/// </summary>
public class Field : SingletonMono<Field>
{

    [Tooltip("エリア左上")]
    [SerializeField] Vector2 _topLeft;
    [Tooltip("エリア右下")]
    [SerializeField] Vector2 _bottomRight;

    Subject<float> _moveSubject = new Subject<float>();
    float _position = 0.0f;

    public IObservable<float> MoveSubject => _moveSubject;

    public IObservable<EreaState> EreaSubject(Transform transform)
    {
        var state = EreaCheck(transform);
        var subject = gameObject.UpdateAsObservable()
            .Where(_ =>
            {
                var buf = EreaCheck(transform);
                if (buf != state)
                {
                    state = buf;
                    return true;
                }
                return false;
            })
            .Select(_ => state);

        return subject;
    }

    public float Position
    {
        get => _position;
        set
        {
            _moveSubject.OnNext(value - _position);
            _position = value;
        }
    }

    public Vector2 Center { get => Vector2.Lerp(Vertex1, Vertex3, 0.5f); }

    public Vector2 Vertex1 { get => _topLeft + Vector2.up * _position; }
    public Vector2 Vertex3 { get => _bottomRight + Vector2.up * _position; }
    public Vector2 Vertex2 { get => new Vector2(_bottomRight.x, _topLeft.y + _position); }
    public Vector2 Vertex4 { get => new Vector2(_topLeft.x, _bottomRight.y + _position); }

    public Vector2 BottomRight { get => _bottomRight; set => _bottomRight = value; }
    public Vector2 TopLeft { get => _topLeft; set => _topLeft = value; }

    public EreaState EreaCheck(Vector2 position)
    {

        var result = EreaState.Off;

        var left = Mathf.Min(Vertex1.x, Vertex3.x);
        var right = Mathf.Max(Vertex1.x, Vertex3.x);
        var top = Mathf.Max(Vertex1.y, Vertex3.y);
        var bottom = Mathf.Min(Vertex1.y, Vertex3.y);

        if (position.x >= left && position.x <= right && position.y >= bottom && position.y <= top)
        {
            result = EreaState.Stay;
        }

        var dir1 = Vertex1 - position;
        var dir2 = Vertex3 - position;
        Debug.DrawRay(position, dir1);
        Debug.DrawRay(position, dir2);

        return result;
    }

    public EreaState EreaCheck(Transform transform)
    {
        return EreaCheck((Vector2)transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        DrawFieldLine();
    }

    void DrawFieldLine()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vertex1, Vertex2);
        Gizmos.DrawLine(Vertex2, Vertex3);
        Gizmos.DrawLine(Vertex3, Vertex4);
        Gizmos.DrawLine(Vertex4, Vertex1);
    }
}
