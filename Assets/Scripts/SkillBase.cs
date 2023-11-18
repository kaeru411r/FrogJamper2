using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : ISkill
{
    [SerializeField] float _coolTime;

    bool _isReady = true;
    float _time = 0.0f;

    public bool IsReady => _isReady;

    public void Play()
    {
        _isReady = false;
        _time = _coolTime;
        PlayImpl();
    }

    abstract protected void PlayImpl();

    virtual public void Cooling(float time)
    {
        _time -= time;

        if(_time <= 0)
        {
            _time = 0;
            _isReady = true;
        }
    }
}
