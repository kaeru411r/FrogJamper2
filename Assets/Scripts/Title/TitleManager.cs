using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Button _skillSettingButton;
    [SerializeField] Button _startButton;
    [SerializeField] GameObject _skillSettingView;


    TMP_Text _skillSettingText;
    string _settingText = "ƒXƒLƒ‹‘I‘ð";
    string _appryText = "–ß‚é";
    bool _isSkillSettingView = false;


    // Start is called before the first frame update
    void Start()
    {
        _skillSettingText = _skillSettingButton.GetComponentInChildren<TMP_Text>();
        _skillSettingText.text = _settingText;
        _skillSettingButton.onClick.AddListener(SkillView);
        _startButton.onClick.AddListener(GameStart);
    }

    void SkillView()
    {
        if (_isSkillSettingView)
        {
            _skillSettingText.text = _settingText;
            _startButton.gameObject.SetActive(true);
            _skillSettingView.SetActive(false);
        }
        else
        {
            _skillSettingText.text = _appryText;
            _startButton.gameObject.SetActive(false);
            _skillSettingView.SetActive(true);
        }

        _isSkillSettingView = !_isSkillSettingView;
    }

    void GameStart()
    {
        SceneManager.LoadScene(1);
    }
}
