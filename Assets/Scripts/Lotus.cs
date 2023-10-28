using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lotus : FieldFollowUpObject
{
    [SerializeField] Vector2 _speed = Vector2.zero;
    [SerializeField] Sprite _texture;

    /// <summary>îÒîjâÛéûÇ…åƒÇ—èoÇ∑</summary>
    public Action<Lotus> OnDestroyAction;


    public Vector2 Speed { get => _speed; set => _speed = value; }


    public void RunStart()
    {
        StartCoroutine(Run());
    }


    IEnumerator Run()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            transform.position += (Vector3)_speed * Time.fixedDeltaTime;
        }
    }

    private void OnDestroy()
    {
        OnDestroyAction.Invoke(this);
    }
}
