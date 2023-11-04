using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldTester : Field
{
    [SerializeField] float _pos;

    private void OnValidate()
    {
        Position = _pos;
    }
}
