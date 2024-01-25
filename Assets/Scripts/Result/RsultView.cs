using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class RsultView : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    [SerializeField] TMP_Text _text2;

    // Start is called before the first frame update
    void Start()
    {
        string crear = ScoreModel.Instance.IsCrear ? "Crear" : "Miss";
        ScoreModel.Instance.Score
            .Take(1)
            .Subscribe(score =>
            { _text.text = $"{crear}\nScore : {score}"; });

    }
}
