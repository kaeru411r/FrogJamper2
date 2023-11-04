using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class Field : MonoBehaviour
{
    static Field _instance;

    public static Field Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindAnyObjectByType<Field>();
                if (!_instance)
                {
                    return null;
                }
            }
            return _instance;
        }
    }

    [Tooltip("�G���A����")]
    [SerializeField] Vector2 _topLeft;
    [Tooltip("�G���A�E��")]
    [SerializeField] Vector2 _bottomRight;
    [Tooltip("�v���C�G���A���")]
    [SerializeField] float _playEreaTop;

    Subject<float> _moveSubject = new Subject<float>();
    float _position = 0.0f;

    public IObservable<float> MoveSubject { get { return _moveSubject; } }

    public float Position
    {
        get => _position;
        set
        {
            _moveSubject.OnNext(value - _position);
            _position = value;
        }
    }

    public Vector2 Vertex1 { get => _topLeft; set => _topLeft = value; }
    public Vector2 Vertex3 { get => _bottomRight; set => _bottomRight = value; }
    public Vector2 Vertex2
    {
        get => new Vector2(_bottomRight.x, _topLeft.y);
        set
        {
            _topLeft.y = value.y;
            _bottomRight.x = value.x;
        }
    }
    public Vector2 Vertex4
    {
        get => new Vector2(_topLeft.x, _bottomRight.y);
        set
        {
            _topLeft.x = value.x;
            _bottomRight.y = value.y;
        }
    }

    public float PlayEreaTop { get => _playEreaTop; set => _playEreaTop = value; }

    private void Awake()
    {
        Destroy(_instance);
        _instance = this;
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

        Gizmos.color = Color.blue;
        var point1 = new Vector3(Vertex1.x, _playEreaTop);
        var point2 = new Vector3(Vertex3.x, _playEreaTop);
        Gizmos.DrawLine(point1, point2);
    }
}
