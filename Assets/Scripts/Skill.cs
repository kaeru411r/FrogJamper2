using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skill")]
public class Skill : ScriptableObject, ISkill
{
    [SelectableSerializeReference, SerializeReference]
    ISkill[] _skills;

    public void Play()
    {
        foreach (var skill in _skills)
        {
            skill.Play();
        }
    }
}
