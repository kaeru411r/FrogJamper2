using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using InputSystemAgent;
using UniRx;

/// <summary>
/// スキルを使用する
/// </summary>
public class SkillUser : MonoBehaviour
{
    private void Start()
    {
        InputAgent2.Subscribe("Player", "Skill1", OnSkill1).AddTo(this);
        InputAgent2.Subscribe("Player", "Skill2", OnSkill2).AddTo(this);
        InputAgent2.Subscribe("Player", "Skill3", OnSkill3).AddTo(this);
    }

    private void Update()
    {
        Cooling();
    }

    void Cooling()
    {
        var skillSet = SkillSet.Instance;

        skillSet.Skill1?.Cooling(Time.deltaTime);
        skillSet.Skill2?.Cooling(Time.deltaTime);
        skillSet.Skill2?.Cooling(Time.deltaTime);
    }

    void OnSkill1(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Canceled)
        {
            var skillSet = SkillSet.Instance;
            if (skillSet.Skill1.IsReady)
            {
                skillSet.Skill1.Play();
            }
        }
    }
    void OnSkill2(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Canceled)
        {
            var skillSet = SkillSet.Instance;
            if (skillSet.Skill2.IsReady)
            {
                skillSet.Skill2.Play();
            }
        }
    }
    void OnSkill3(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Canceled)
        {
            var skillSet = SkillSet.Instance;
            if (skillSet.Skill3.IsReady)
            {
                skillSet.Skill3.Play();
            }
        }
    }


}
