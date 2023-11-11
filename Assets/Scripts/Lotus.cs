using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 蓮
/// </summary>
public class Lotus : FieldFollowUpObject
{
    [SerializeField] Vector2 _speed = Vector2.zero;
    [SerializeField] Sprite _texture;

    Subject<float> _moveSubject = new Subject<float>();

    /// <summary>非破壊時に呼び出す</summary>
    public Action<Lotus> OnDestroyAction;
    /// <summary>移動ベクトル</summary>
    public IObservable<float> MoveSubject { get => _moveSubject; }
    /// <summary>移動速度</summary>
    public Vector2 Speed { get => _speed; set => _speed = value; }


    /// <summary>移動開始</summary>
    public void RunStart()
    {
        StartCoroutine(Run());
    }


    IEnumerator Run()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            var vec = _speed * Time.fixedDeltaTime;
            Rigidbody.MovePosition((Vector2)transform.position + vec);
            _moveSubject.OnNext(vec.y);
        }
    }


    protected override void FieldOut()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnDestroyAction?.Invoke(this);
    }
}
