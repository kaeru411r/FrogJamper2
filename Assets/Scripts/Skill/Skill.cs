using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skill")]
public class Skill : ScriptableObject
{
    [SerializeField] float _coolTime;
    [SelectableSerializeReference, SerializeReference]
    ISkill[] _skills;

    float _time = 0.0f;
    bool _isReady = false;

    public ISkill[] Skills { get => _skills;}

    public bool IsReady => _isReady;


    public void Cooling(float time)
    {
        _time -= time;

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
