using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSet : SingletonMono<SkillSet>
{
    protected override bool isDontDestroyOnLoad => true;

    [Tooltip("スキル1")]
    [SerializeField] Skill _skill1;
    [Tooltip("スキル2")]
    [SerializeField] Skill _skill2;
    [Tooltip("スキル3")]
    [SerializeField] Skill _skill3;

    public Skill Skill1 { get => _skill1; set => _skill1 = value; }
    public Skill Skill2 { get => _skill2; set => _skill2 = value; }
    public Skill Skill3 { get => _skill3; set => _skill3 = value; }
}
