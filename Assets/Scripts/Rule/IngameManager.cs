using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UnityEngine.InputSystem.Utilities;
using InputSystemAgent;
using System.Linq;

public class IngameManager : SingletonMono<IngameManager>
{
    override protected bool isDontDestroyOnLoad => true;


    [Tooltip("‰Šúƒ‰ƒCƒt")]
    [SerializeField] int _initLife;
    [Tooltip("’¾‚ñ‚Å‚¢‚éŽžŠÔ")]
    [SerializeField] float _drowingTime;

    IngameViewModel _ingameViewModel;
    int _life;
    bool _isInitialize = false;

    public IngameViewModel.IngameViewModelWrapper ViewModel
    {
        get
        {
            if (_ingameViewModel == null)
            {
                Initialize();
            }
            return _ingameViewModel.Wrapper;
        }
    }

    private void Start()
    {
        Initialize();
    }

    new public void Initialize()
    {
        if (_isInitialize) { return; }

        InputAgent2.InputMaps
            .Where(map => map.name == "Player")
            .FirstOrDefault()
            .Enable();

        _ingameViewModel = new IngameViewModel();
        _life = _initLife;
        _ingameViewModel.SetLife(_life);

        _isInitialize = true;
    }

    public void Goal()
    {
        InputAgent2.InputMaps
            .Where(map => map.name == "Player")
            .FirstOrDefault()
            .Disable();
        _ingameViewModel.GameEnd();
    }

    public void Drow()
    {
        StartCoroutine(Drowing());
    }

    IEnumerator Drowing()
    {
        var time = _drowingTime;
        while (time > 0)
        {
            yield return null;

            time -= Time.deltaTime;
        }

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
