using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [Tooltip("PlayerInput")]
    [SerializeField] PlayerInput _playerInput;

    public Skill Skill1 { get => _skill1; set => _skill1 = value; }
    public Skill Skill2 { get => _skill2; set => _skill2 = value; }
    public Skill Skill3 { get => _skill3; set => _skill3 = value; }

    private void Start()
    {
        _playerInput.AddListener("Player", "Skill1", OnSkill1);
        _playerInput.AddListener("Player", "Skill2", OnSkill2);
        _playerInput.AddListener("Player", "Skill3", OnSkill3);
    }

    void OnSkill1(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Canceled)
        {
            _skill1.Play();
        }
    }
    void OnSkill2(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Canceled)
        {
            _skill2.Play();
        }
    }
    void OnSkill3(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Canceled)
        {
            _skill3.Play();
        }
    }
}
