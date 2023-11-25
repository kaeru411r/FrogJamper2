using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Xml;

/// <summary>
/// 蓮
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Lotus : MonoBehaviour
{
    [SerializeField] Vector2 _speed = Vector2.zero;
    [SerializeField] Sprite _texture;

    Rigidbody2D _rigidbody;
    bool _isRun = false;
    Subject<Lotus> _onDestroyed = new Subject<Lotus>();

    /// <summary>非破壊時に呼び出す</summary>
    public IObservable<Lotus> OnDestroyed => _onDestroyed;
    /// <summary>移動速度</summary>
    public Vector2 Speed { get => _speed; set => _speed = value; }
    public Rigidbody2D Rigidbody => _rigidbody;


    /// <summary>移動開始</summary>
    public void RunStart()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = _speed;
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
