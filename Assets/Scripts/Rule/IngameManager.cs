using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UnityEngine.InputSystem.Utilities;

public class IngameManager : SingletonMono<IngameManager>
{
    override protected bool isDestroyOnLoad { get { return true; } }


    [Tooltip("初期ライフ")]
    [SerializeField] int _initLife;
    [Tooltip("沈んでいる時間")]
    [SerializeField] float _drowingTime;

    IngameViewModel _ingameViewModel;
    int _life;

    public IngameViewModel.IngameViewModelWrapper ViewModel
    {
        get
        {
            if (_ingameViewModel == null)
            {
                _ingameViewModel = new IngameViewModel();
                Initialize();
            }
            return _ingameViewModel.Wrapper;
        }
    }


    new public void Initialize()
    {
        _life = _initLife;
        _ingameViewModel.SetLife(_life);
    }

    public void Drowing()
    {
        Debug.Log(_life);
        if (_life > 0)
        {
            _life--;
            _ingameViewModel.SetLife(_life);
            _ingameViewModel.Drown();
        }
        else
        {
            _ingameViewModel.GameEnd();
        }
    }
}
