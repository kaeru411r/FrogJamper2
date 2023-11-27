using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;

public class ScoreView : MonoBehaviour
{
    [Tooltip("ÉXÉRÉAÇÃUI")]
    [SerializeField] TMP_Text _Text;
    // Start is called before the first frame update
    void Start()
    {
        ScoreModel.Instance?.Score
            .Subscribe(ScoreUpdate)
            .AddTo(this);
        _Text.text = 0.ToString();
    }

    void ScoreUpdate(int score)
    {
        if (_Text)
        {
            _Text.text = score.ToString();
        }
    }
}
