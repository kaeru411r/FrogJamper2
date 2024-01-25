using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEditor;

public class ScoreModel : SingletonMono<ScoreModel>
{
    protected override bool isDontDestroyOnLoad => true;

    ReactiveProperty<int> _score = new ReactiveProperty<int>();
    float _scoreFactor = 1.0f;
    bool _isCrear = false;

    public IObservable<int> Score => _score;

    public float ScoreFactor { get => _scoreFactor; set => _scoreFactor = value; }
    public bool IsCrear { get => _isCrear; set => _isCrear = value; }

    public void AddScore(int value)
    {
        _score.Value += (int)(value * _scoreFactor);
    }

    public void Reset()
    {
        _scoreFactor = 1.0f;
        _score.Value = 0;
        IsCrear = false;
    }
}
