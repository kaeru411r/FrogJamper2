using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ChargeBoost : ISkill
{
    [Tooltip("Œø‰ÊŽžŠÔ")]
    [SerializeField] float _time;
    [Tooltip("”{—¦")]
    [SerializeField] float _boost;

    Frog _frog;

    public void Play()
    {
        Debug.Log("ChargeUp");
        _frog = Frog.Instance;
        if (_frog)
        {
            _frog.ChargeSpeed *= _boost;
            Observable.Timer(TimeSpan.FromSeconds(_time))
                .Subscribe(_ => Cancel())
                .AddTo(_frog);
        }
    }

    void Cancel()
    {
        Debug.Log("ChargeDown");
        _frog.ChargeSpeed /= _boost;
    }
}
