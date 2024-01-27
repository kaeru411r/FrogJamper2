using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skill")]
public class Skill : ScriptableObject
{
    [SerializeField] float _coolTime;
    [SerializeField] string _name;
    [TextArea(1, 5)]
    [SerializeField] string _tips;
    [SelectableSerializeReference, SerializeReference]
    ISkill[] _skills = new ISkill[0];

    float _time = 0.0f;
    bool _isReady = false;
    ReactiveProperty<float> _coolPar = new ReactiveProperty<float>();

    public ISkill[] Skills { get => _skills;}

    public bool IsReady => _isReady;

    public string Name { get => _name; set => _name = value; }
    public string Tips { get => _tips; set => _tips = value; }
    public IObservable<float> CoolPar => _coolPar;

    public void Cooling(float time)
    {
        _time -= time;
        _coolPar.Value = _coolTime > 0 ? _time / _coolTime : 0.0f;

        if (_time <= 0)
        {
            _time = 0;
            _isReady = true;
        }
    }

    public void Play()
    {
        _isReady = false;
        _time = _coolTime;
        foreach (var skill in _skills)
        {
            skill.Play();
        }
    }
}
