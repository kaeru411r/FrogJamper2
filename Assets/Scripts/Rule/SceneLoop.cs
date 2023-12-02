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
        Frog.Instance.OnState
            .Where(state => state == FrogState.Drown)
            .Subscribe(_ => Restart())
            .AddTo(this);
    }


    void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
