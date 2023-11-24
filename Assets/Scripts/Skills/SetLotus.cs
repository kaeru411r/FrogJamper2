using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLotus : ISkill
{
    public void Play()
    {
        Debug.Log("LotusSet");
        LotusManager.Instance.Generate(Frog.Instance.transform.position);
    }
}
