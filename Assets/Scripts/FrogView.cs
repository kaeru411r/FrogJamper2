using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

[RequireComponent(typeof(SpriteRenderer), typeof(Frog))]
public class FrogView : MonoBehaviour
{
    [Tooltip("立ってるカエル")]
    [SerializeField] Sprite _stand;
    [Tooltip("飛んでるカエル")]
    [SerializeField] Sprite _jump;
    [Tooltip("溺れてるカエル")]
    [SerializeField] Sprite _drown;

    Frog _frog;
    SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _frog = GetComponent<Frog>();
        _frog.StateSubject.Subscribe(Alteration).AddTo(this);
    }


    public void Alteration(FrogState state)
    {
        switch (state)
        {
            case FrogState.Stand:
                _spriteRenderer.sprite = _stand;
                break;
            case FrogState.Jump:
                _spriteRenderer.sprite = _jump;
                break;
            case FrogState.Drown:
                _spriteRenderer.sprite = _drown;
                break;
            default:
                break;
        }
    }
}
