using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class Field : MonoBehaviour
{

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
}
