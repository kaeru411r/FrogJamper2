using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoop : MonoBehaviour
{
    [SerializeField] int _resultSceneIndex;

    public int ResultSceneIndex { get => _resultSceneIndex; set => _resultSceneIndex = Mathf.Max(value, 0); }

    // Start is called before the first frame update
    void Start()
    {
        IngameManager.Instance.ViewModel.OnGameEnd
            .Subscribe(_ => Result())
            .AddTo(this);
        IngameManager.Instance.ViewModel.OnDrown
            .Subscribe(_ => Restart())
            .AddTo(this);
    }


    void Restart()
    {
        SceneChanger.ReloadScene();
    }

    void Result()
    {
        SceneChanger.ChangeScene(ResultSceneIndex);
    }

    private void OnValidate()
    {
        _resultSceneIndex = Mathf.Max(_resultSceneIndex, 0);
    }
}
