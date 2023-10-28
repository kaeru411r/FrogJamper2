using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class FieldFollowUpObject : MonoBehaviour
{
    static Field _field;

    static Field Field
    {
        get
        {
            if(!_field)
            {
                _field = FindAnyObjectByType<Field>();
                if(!_field)
                {
                    return null;
                }
            }
            return _field;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(Field != null)
        {
            Field.MoveSubject.Subscribe(Move).AddTo(this);
        }
    }

    void Move(float distance)
    {
        transform.position += Vector3.down * distance;
    }
}
