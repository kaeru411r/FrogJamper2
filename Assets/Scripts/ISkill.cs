using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�L���̃C���^�[�t�F�[�X
/// </summary>
public interface ISkill
{
    public bool IsReady { get; }

    public void Play();
    public void Cooling(float time);
}
