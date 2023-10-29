using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class FieldFollowUpObject : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        if(Field.Instance != null)
        {
            Field.Instance.MoveSubject.Subscribe(Move).AddTo(this);
        }
    }

    void Move(float distance)
    {
        transform.position += Vector3.down * distance;
    }
}
