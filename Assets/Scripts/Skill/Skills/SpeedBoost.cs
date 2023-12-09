using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


/// <summary>
/// �W�����v���x���グ��
/// </summary>
public class SpeedBoost : ISkill
{
    [Tooltip("���ʎ���")]
    [SerializeField] float _time;
    [Tooltip("�{��")]
    [SerializeField] float _boost;

    Frog _frog;

    public void Play()
    {
        Debug.Log("SpeedUp");
        _frog = Frog.Instance;
        Debug.Log(_frog.transform);
        if (_frog)
        {
            _frog.Speed *= _boost;
            Observable.Timer(TimeSpan.FromSeconds(_time))
                .Subscribe(_ => Cancel())
                .AddTo(_frog);
        }
    }

    void Cancel()
    {
        Debug.Log("SpeedDown");
        _frog.Speed /= _boost;
    }
}
