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
    [Tooltip("スキル1")]
    [SerializeField] Skill _skill1;
    [Tooltip("スキル2")]
    [SerializeField] Skill _skill2;
    [Tooltip("スキル3")]
    [SerializeField] Skill _skill3;

    public Skill Skill1 { get => _skill1; set => _skill1 = value; }
    public Skill Skill2 { get => _skill2; set => _skill2 = value; }
    public Skill Skill3 { get => _skill3; set => _skill3 = value; }

    private void Start()
    {
        InputAgent2.Subscribe("Player", "Skill1", OnSkill1).AddTo(this);
        InputAgent2.Subscribe("Player", "Skill2", OnSkill2).AddTo(this);
        InputAgent2.Subscribe("Player", "Skill3", OnSkill3).AddTo(this);
    }

    private void Update()
    {
        _skill1.Cooling(Time.deltaTime);
        _skill2.Cooling(Time.deltaTime);
        _skill3.Cooling(Time.deltaTime);
    }

    void OnSkill1(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Canceled)
        {
            if (_skill1.IsReady)
            {
                _skill1.Play();
            }
        }
    }
    void OnSkill2(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Canceled)
        {
            if (_skill2.IsReady)
            {
                _skill2.Play();
            }
        }
    }
    void OnSkill3(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Canceled)
        {
            if (_skill3.IsReady)
            {
                _skill3.Play();
            }
        }
    }


}
