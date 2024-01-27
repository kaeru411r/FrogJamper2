using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class SkillButtonView : MonoBehaviour
{
    [SerializeField] Image _skill1Overwrapimage;
    [SerializeField] Image _skill2Overwrapimage;
    [SerializeField] Image _skill3Overwrapimage;
    // Start is called before the first frame update
    void Start()
    {
        _skill1Overwrapimage.type = Image.Type.Filled;
        _skill1Overwrapimage.fillMethod = Image.FillMethod.Radial360;
        SkillSet.Instance.Skill1.CoolPar
            .Subscribe(value => _skill1Overwrapimage.fillAmount = value)
            .AddTo(this);
        _skill2Overwrapimage.type = Image.Type.Filled;
        _skill2Overwrapimage.fillMethod = Image.FillMethod.Radial360;
        SkillSet.Instance.Skill2.CoolPar
            .Subscribe(value => _skill2Overwrapimage.fillAmount = value)
            .AddTo(this);
        _skill3Overwrapimage.type = Image.Type.Filled;
        _skill3Overwrapimage.fillMethod = Image.FillMethod.Radial360;
        SkillSet.Instance.Skill3.CoolPar
            .Subscribe(value => _skill3Overwrapimage.fillAmount = value)
            .AddTo(this);
    }
}
