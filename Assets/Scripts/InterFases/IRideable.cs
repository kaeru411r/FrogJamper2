using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRideable
{
    public int Hierarchy { get; }
    public Rigidbody2D Rigidbody { get; }
    public Vector2 Position { get; }
    /// <summary>”ñ”j‰ó‚ÉŒÄ‚Ño‚·</summary>
    public IObservable<IRideable> OnDestroyed { get; }

    public IObservable<Vector2> Ride();
}
