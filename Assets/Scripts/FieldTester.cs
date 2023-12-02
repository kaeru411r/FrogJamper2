using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class FieldTester : Field
{
    [SerializeField] float _pos;

    private void Start()
    {
        MoveSubject
            .Subscribe(value => _pos += value)
            .AddTo(this);
    }

    private void OnValidate()
    {
        Position = _pos;
    }
}
