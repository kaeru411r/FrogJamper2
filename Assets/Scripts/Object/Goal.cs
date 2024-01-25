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


    public int Hierarchy => _hierarchy;

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

    public Vector2 Position => transform.position;

    public IObservable<Vector2> Ride()
    {
        Debug.Log("ÉSÅ[Éã");

        ScoreModel.Instance.IsCrear = true;

        var subject = new Subject<Vector2>();
        subject.OnNext(transform.position);

        IngameManager.Instance.Goal();

        return subject;
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
