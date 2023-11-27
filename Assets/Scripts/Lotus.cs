using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Xml;

/// <summary>
/// �@
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Lotus : MonoBehaviour, IRideable
{
    [SerializeField] Vector2 _speed = Vector2.zero;
    [SerializeField] Sprite _texture;
    [SerializeField] int _hierarchy = 0;
    [SerializeField] int _score = 0;

    Rigidbody2D _rigidbody;
    Subject<Lotus> _onDestroyed = new Subject<Lotus>();
    bool _isRided = false;

    /// <summary>��j�󎞂ɌĂяo��</summary>
    public IObservable<Lotus> OnDestroyed => _onDestroyed;
    /// <summary>�ړ����x</summary>
    public Vector2 Speed { get => _speed; set => _speed = value; }
    public Rigidbody2D Rigidbody => _rigidbody;

    public int Hierarchy => _hierarchy;

    public void Ride()
    {
        if (!_isRided)
        {
            _isRided = true;
            ScoreModel.Instance.AddScore(_score);
        }
    }


    /// <summary>�ړ��J�n</summary>
    public void RunStart()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = _speed;
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        Field.Instance.EreaSubject(transform)
            .Where(state => state == EreaState.Off)
            .Subscribe(_ => Destroy(gameObject))
            .AddTo(this);
    }
    private void OnDestroy()
    {
        _onDestroyed.OnNext(this);
        _onDestroyed.OnCompleted();
    }
}
