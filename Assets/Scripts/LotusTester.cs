using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusTester : MonoBehaviour
{
    [SerializeField] LotusManager _lotusManager;
    // Start is called before the first frame update
    void Start()
    {
        _lotusManager.GenerateStart();
    }

}
