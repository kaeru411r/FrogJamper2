using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        IngameManager.Instance.ViewModel.OnGameEnd
            .Subscribe(_ => Restart())
            .AddTo(this);
        IngameManager.Instance.ViewModel.OnDrown
            .Subscribe(_ => Restart())
            .AddTo(this);
    }


    void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
