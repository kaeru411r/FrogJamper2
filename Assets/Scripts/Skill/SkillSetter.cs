using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SkillSetter : MonoBehaviour
{
    [SerializeField] Skill[] _skills;
    [SerializeField] RectTransform _skillView;
    [SerializeField] TMP_Text _skillText;
    [SerializeField] Button _skillSetButton;
    [SerializeField] Button _skill1Button;
    [SerializeField] Button _skill2Button;
    [SerializeField] Button _skill3Button;
    [SerializeField] TMP_Text _skill1Name;
    [SerializeField] TMP_Text _skill2Name;
    [SerializeField] TMP_Text _skill3Name;

    Skill _selectSkill;

    // Start is called before the first frame update
    void Start()
    {
        _skill1Button.onClick.AddListener(() => SkillSetup(1));
        _skill1Name.text = SkillSet.Instance.Skill1 != null ? SkillSet.Instance.Skill1.Name : "";
        _skill2Button.onClick.AddListener(() => SkillSetup(2));
        _skill2Name.text = SkillSet.Instance.Skill2 != null ? SkillSet.Instance.Skill2.Name : "";
        _skill3Button.onClick.AddListener(() => SkillSetup(3));
        _skill3Name.text = SkillSet.Instance.Skill3 != null ? SkillSet.Instance.Skill3.Name : "";

        foreach (var skill in _skills)
        {
            var button = Instantiate(_skillSetButton, _skillView);
            button.onClick.AddListener(() => SkillSelect(skill));
            var text = button.GetComponentInChildren<TMP_Text>();
            text.text = skill.Name;
        }
    }

    void SkillSelect(Skill skill)
    {
        _selectSkill = skill;
        _skillText.text = $"{skill.Name}\n{skill.Tips}";
    }

    void SkillSetup(int skillIndex)
    {
        if (skillIndex == 1)
        {
            SkillSet.Instance.Skill1 = _selectSkill;
            _skill1Name.text = _selectSkill.Name;
        }
        else if (skillIndex == 2)
        {
            SkillSet.Instance.Skill2 = _selectSkill;
            _skill2Name.text = _selectSkill.Name;
        }
        else if (skillIndex == 3)
        {
            SkillSet.Instance.Skill3 = _selectSkill;
            _skill3Name.text = _selectSkill.Name;
        }
    }
}
