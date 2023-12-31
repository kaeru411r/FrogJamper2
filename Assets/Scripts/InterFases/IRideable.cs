using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRideable
{
    public int Hierarchy { get; }
    public Rigidbody2D Rigidbody { get; }
    public Vector2 Position { get; }
    /// <summary>非破壊時に呼び出す</summary>
    public IObservable<IRideable> OnDestroyed { get; }

    public IObservable<Vector2> Ride();
}
