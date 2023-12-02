using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// フィールドに追従するオブジェクト
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class FieldFollowUpObject : MonoBehaviour
{

    public Rigidbody2D Rigidbody { get; private set; }

    // Start is called before the first frame update
    protected void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        if (Field.Instance)
        {
            Field.Instance.MoveSubject.Subscribe(Move).AddTo(this);
        }
    }


    void Move(float distance)
    {
        transform.Translate(Vector2.down * distance);
        //Rigidbody.MovePosition((Vector2)transform.position + ());
    }

}
