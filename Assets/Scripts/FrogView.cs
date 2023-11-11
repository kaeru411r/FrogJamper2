using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;


/// <summary>
/// �J�G���̌�����
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class FrogView : MonoBehaviour
{
    [Tooltip("�����Ă�J�G��")]
    [SerializeField] Sprite _stand;
    [Tooltip("���ł�J�G��")]
    [SerializeField] Sprite _jump;
    [Tooltip("�M��Ă�J�G��")]
    [SerializeField] Sprite _drown;

    SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (TryGetComponent(out Frog frog)) {
            frog.StateSubject.Subscribe(Alteration).AddTo(this);
        }
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
