using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スキルのインターフェース
/// </summary>
public interface ISkill
{
    public bool IsReady { get; }

    public void Play();
    public void Cooling(float time);
}
