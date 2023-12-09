using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Diagnostics;

[RequireComponent(typeof(Rigidbody2D))]
public class Goal : MonoBehaviour, IRideable
{
    [Tooltip("ï\é¶èá")]
    [SerializeField] int _hierarchy = 0;

    Rigidbody2D _rigidbody;
    Subject<IRideable> _onDestroyed = new Subject<IRideable>();
    Subject<Unit> _onGoal = new Subject<Unit>();


    public int Hierarchy => _hierarchy;

    public Transform Transform => transform;

    public Rigidbody2D Rigidbody
    {
        get
        {
            if (!_rigidbody)
            {
                _rigidbody = GetComponent<Rigidbody2D>();
            }
            return _rigidbody;
        }
    }

    public IObservable<IRideable> OnDestroyed => _onDestroyed;

    public IObservable<Unit> OnGoal => _onGoal;

    public void Ride()
    {
        Debug.Log("ÉSÅ[Éã");
        _onGoal.OnNext(Unit.Default);
        _onGoal.OnCompleted();
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0;
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnDestroy()
    {
        _onDestroyed.OnNext(this);
    }
}
