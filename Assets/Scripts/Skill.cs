using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skill")]
public class Skill : ScriptableObject
{
    [SelectableSerializeReference, SerializeReference]
    ISkill[] _skills;

    public ISkill[] Skills { get => _skills;}

    public bool IsReady => Skills.Where(skill =>  skill.IsReady).Any();

    public void Cooling(float time)
    {
        foreach (var skill in _skills)
        {
            skill.Cooling(time);
        }
    }

    public void Play()
    {
        foreach (var skill in _skills)
        {
            skill.Play();
        }
    }
}
