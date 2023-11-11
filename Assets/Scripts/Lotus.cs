using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// �@
/// </summary>
public class Lotus : FieldFollowUpObject
{
    [SerializeField] Vector2 _speed = Vector2.zero;
    [SerializeField] Sprite _texture;

    Subject<float> _moveSubject = new Subject<float>();

    /// <summary>��j�󎞂ɌĂяo��</summary>
    public Action<Lotus> OnDestroyAction;
    /// <summary>�ړ��x�N�g��</summary>
    public IObservable<float> MoveSubject { get => _moveSubject; }
    /// <summary>�ړ����x</summary>
    public Vector2 Speed { get => _speed; set => _speed = value; }


    /// <summary>�ړ��J�n</summary>
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
