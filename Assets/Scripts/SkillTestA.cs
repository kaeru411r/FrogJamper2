using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTestA : ISkill
{
    [SerializeField]
    string _text;
    public void Play()
    {
        Debug.Log(_text);
    }
}
