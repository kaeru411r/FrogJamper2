using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class IngameViewModel
{
    IngameViewModelWrapper _wrapper;
    Subject<Unit> _onGameEnd = new Subject<Unit>();
    ReactiveProperty<int> _onLife = new ReactiveProperty<int>();
    Subject<Unit> _onDrown = new Subject<Unit>();


    public IngameViewModelWrapper Wrapper
    {
        get
        {
            if (_wrapper == null)
            {
                _wrapper = new IngameViewModelWrapper(this);
            }
            return _wrapper;
        }
    }

    public IObservable<Unit> OnGameEnd => _onGameEnd;
    public IObservable<int> OnLife => _onLife;
    public IObservable<Unit> OnDrown => _onDrown;

    public void GameEnd()
    {
        _onGameEnd.OnNext(Unit.Default);
        _onGameEnd.OnCompleted();
    }

    public void SetLife(int value)
    {
        _onLife.Value = value;
    }

    public void Drown()
    {
        _onDrown.OnNext(Unit.Default);
    }

    public class IngameViewModelWrapper
    {
        IngameViewModel _viewModel;

        private IngameViewModelWrapper() { }

        public IngameViewModelWrapper(IngameViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public IObservable<Unit> OnGameEnd => _viewModel.OnGameEnd;
        public IObservable<int> OnLife => _viewModel.OnLife;
        public IObservable<Unit> OnDrown => _viewModel.OnDrown;
    }
}
