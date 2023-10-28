using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldTester : Field
{
    [SerializeField] float _position;

    private void OnValidate()
    {
        Position = _position;
    }
}
