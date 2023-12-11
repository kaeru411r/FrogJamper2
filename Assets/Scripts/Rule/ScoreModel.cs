using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEditor;

public class ScoreModel : SingletonMono<ScoreModel>
{
    ReactiveProperty<int> _score = new ReactiveProperty<int>();
    float _scoreFactor = 1.0f;

    public IObservable<int> Score => _score;

    public float ScoreFactor { get => _scoreFactor; set => _scoreFactor = value; }

    public void AddScore(int value)
    {
        _score.Value += (int)(value * _scoreFactor);
    }
}
