using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTestA : SkillBase
{
    [SerializeField]
    string _text;
    override protected void PlayImpl()
    {
        Debug.Log(_text);
    }
}
