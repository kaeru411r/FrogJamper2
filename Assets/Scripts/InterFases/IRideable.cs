using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRideable
{
    public int Hierarchy { get; }
    public Transform Transform { get; }
    public Rigidbody2D Rigidbody { get; }
    /// <summary>”ñ”j‰ó‚ÉŒÄ‚Ño‚·</summary>
    public IObservable<IRideable> OnDestroyed { get; }

    public void Ride();
}
