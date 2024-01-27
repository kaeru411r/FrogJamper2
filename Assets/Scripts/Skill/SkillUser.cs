using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using InputSystemAgent;
using UniRx;
using UnityEngine.UI;

/// <summary>
/// スキルを使用する
/// </summary>
public class SkillUser : MonoBehaviour
{
    [SerializeField] Button _skill1Button;
    [SerializeField] Button _skill2Button;
    [SerializeField] Button _skill3Button;
    private void Start()
    {
        InputAgent2.Subscribe("Player", "Skill1", OnSkill1).AddTo(this);
        InputAgent2.Subscribe("Player", "Skill2", OnSkill2).AddTo(this);
        InputAgent2.Subscribe("Player", "Skill3", OnSkill3).AddTo(this);
        _skill1Button.onClick.AddListener(() => UseSkill(1));
        _skill2Button.onClick.AddListener(() => UseSkill(2));
        _skill3Button.onClick.AddListener(() => UseSkill(3));
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
            UseSkill(1);
        }
    }
    void OnSkill2(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Canceled)
        {
            UseSkill(2);
        }
    }
    void OnSkill3(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Canceled)
        {
            UseSkill(3);
        }
    }

    void UseSkill(int skill)
    {
        var skillSet = SkillSet.Instance;
        if (skill == 1)
        {
            if (skillSet.Skill1.IsReady)
            {
                skillSet.Skill1.Play();
            }
        }
        if (skill == 2)
        {
            if (skillSet.Skill2.IsReady)
            {
                skillSet.Skill2.Play();
            }
        }
        if (skill == 3)
        {
            if (skillSet.Skill3.IsReady)
            {
                skillSet.Skill3.Play();
            }
        }
    }


}
