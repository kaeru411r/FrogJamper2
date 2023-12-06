using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// �t�B�[���h�ɒǏ]����I�u�W�F�N�g
/// </summary>
public class FieldFollowUpObject : MonoBehaviour
{
    IDisposable _subject;

    private void OnEnable()
    {
        _subject = Field.Instance?.MoveSubject.Subscribe(Move).AddTo(this);
    }

    private void OnDisable()
    {
        _subject?.Dispose();
    }

    void Move(float distance)
    {
        transform.Translate(Vector2.down * distance);
    }

}
