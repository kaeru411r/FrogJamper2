using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;
using UnityEngine.Networking.PlayerConnection;

/// <summary>
/// カエルの見た目
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class FrogView : MonoBehaviour
{
    [Tooltip("立ってるカエル")]
    [SerializeField] Sprite _stand;
    [Tooltip("飛んでるカエル")]
    [SerializeField] Sprite _jump;
    [Tooltip("溺れてるカエル")]
    [SerializeField] Sprite _drown;
    [Tooltip("着地予想点")]
    [SerializeField] Image _sight;

    SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (TryGetComponent(out Frog frog))
        {
            frog.OnState.Subscribe(Alteration).AddTo(this);
            frog.JumpDistance.Subscribe(Sight).AddTo(this);
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

    public void Sight(float distance)
    {
        if (distance <= 0)
        {
            _sight.enabled = false;
            return;
        }
        else
        {
            _sight.enabled = true;
            var x = distance * Mathf.Sin(-transform.eulerAngles.z /180 * Mathf.PI);
            var y = distance * Mathf.Cos(-transform.eulerAngles.z / 180 * Mathf.PI);
            var pos = transform.position + new Vector3(x, y, 0);
            Debug.DrawLine(pos, transform.position);
            Debug.Log(pos);
            _sight.transform.position = pos;
        }


    }
}
