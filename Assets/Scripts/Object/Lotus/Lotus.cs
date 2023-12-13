using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Xml;
using UniRx.Triggers;

/// <summary>
/// 蓮
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Lotus : MonoBehaviour, IRideable
{
    [SerializeField] Vector2 _velocity = Vector2.zero;
    [SerializeField] Sprite _texture;
    [SerializeField] int _hierarchy = 0;
    [SerializeField] int _score = 0;

    Rigidbody2D _rigidbody;
    Subject<IRideable> _onDestroyed = new Subject<IRideable>();
    bool _isRided = false;
    Subject<Vector2> _onPosition = new Subject<Vector2>();

    /// <summary>非破壊時に呼び出す</summary>
    public IObservable<IRideable> OnDestroyed => _onDestroyed;
    /// <summary>移動速度</summary>
    public Vector2 Velocity { get => _velocity; set => _velocity = value; }
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
    public int Hierarchy => _hierarchy;

    public Vector2 Position => transform.position;

    public IObservable<Vector2> Ride()
    {
        if (!_isRided)
        {
            _isRided = true;
            ScoreModel.Instance.AddScore(_score);
        }
        return _onPosition;
    }


    /// <summary>移動開始</summary>
    public void RunStart()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        Field.Instance.EreaSubject(transform)
            .Where(state => state == EreaState.Off)
            .Subscribe(_ => Destroy(gameObject))
            .AddTo(this);
        StartCoroutine(Running());
    }

    IEnumerator Running()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            transform.Translate((Vector3)_velocity * Time.fixedDeltaTime, Space.World);
            _onPosition.OnNext(transform.position);
        }
    }


    private void OnDestroy()
    {
        _onDestroyed.OnNext(this);
        _onDestroyed.OnCompleted();
    }
}
