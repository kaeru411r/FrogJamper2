using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSetter : MonoBehaviour
{
    [SerializeField] Skill[] _skills;
    [SerializeField] RectTransform _skillView;
    [SerializeField] Button _skillSetButton;
    [SerializeField] Button _skill1Button;
    [SerializeField] Button _skill2Button;
    [SerializeField] Button _skill3Button;

    int _skillIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
        _skill1Button.onClick.AddListener(() => _skillIndex = 1);
        _skill2Button.onClick.AddListener(() => _skillIndex = 2);
        _skill3Button.onClick.AddListener(() => _skillIndex = 3);

        foreach (var skill in _skills)
        {
            var button = Instantiate(_skillSetButton, _skillView);
            button.onClick.AddListener(() => SkilSet(skill));
            var text = button.GetComponentInChildren<TMP_Text>();
            text.text = skill.name;
        }
    }

    void SkilSet(Skill skill)
    {
        if (_skillIndex == 1)
        {
            SkillSet.Instance.Skill1 = skill;
        }
        else if (_skillIndex == 2)
        {
            SkillSet.Instance.Skill2 = skill;
        }
        else if (_skillIndex == 3)
        {
            SkillSet.Instance.Skill3 = skill;
        }
    }
}
