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

    [Tooltip("エリア左上")]
    [SerializeField] Vector2 _topLeft;
    [Tooltip("エリア右下")]
    [SerializeField] Vector2 _bottomRight;

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

    public Vector2 TopLeft { get => _topLeft; set => _topLeft = value; }
    public Vector2 BottomRight { get => _bottomRight; set => _bottomRight = value; }



    private void Awake()
    {
        Destroy(_instance);
        _instance = this;
    }
}
