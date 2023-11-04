using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(Rigidbody2D))]
public class FieldFollowUpObject : MonoBehaviour
{
    EreaState _ereaState = EreaState.Lost;

    protected Rigidbody2D _rigidbody { get; private set; }

    virtual protected void FieldOut() { }

    virtual protected void FieldIn() { }

    virtual protected void FieldLost() { }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        if (!Field.Instance)
        {
            Field.Instance.MoveSubject.Subscribe(Move).AddTo(this);
        }
        _ereaState = EreaCheck();
        StartCoroutine(EreaCheckLoop());
    }

    IEnumerator EreaCheckLoop()
    {
        while (true)
        {
            var state = EreaCheck();

            if (_ereaState != state)
            {
                if (state == EreaState.Off)
                {
                    FieldOut();
                }
                else if (state == EreaState.Stay)
                {
                    FieldIn();
                }
                else if (state == EreaState.Lost)
                {
                    FieldLost();
                }

                _ereaState = state;
            }
            yield return null;

        }
    }

    void Move(float distance)
    {
        _rigidbody.MovePosition((Vector2)transform.position + (Vector2.down * distance));
    }

    EreaState EreaCheck()
    {
        if (!Field.Instance) { return EreaState.Lost; }

        var result = EreaState.Off;

        var left = Mathf.Min(Field.Instance.Vertex1.x, Field.Instance.Vertex3.x);
        var right = Mathf.Max(Field.Instance.Vertex1.x, Field.Instance.Vertex3.x);
        var top = Mathf.Max( Field.Instance.Vertex1.y, Field.Instance.Vertex3.y);
        var bottom = Mathf.Min( Field.Instance.Vertex1.y, Field.Instance.Vertex3.y);

        var pos = transform.position;

        if(pos.x >= left && pos.x <= right && pos.y >= bottom && pos.y <= top)
        {
            result = EreaState.Stay;
        }

        var dir1 = Field.Instance.Vertex1 - (Vector2)pos;
        var dir2 = Field.Instance.Vertex3 - (Vector2)pos;
        Debug.DrawRay(pos, dir1);
        Debug.DrawRay(pos, dir2);

        return result;
    }

    enum EreaState
    {
        Lost = -1,

        Stay = 0,
        Off = 1,

    }

}
