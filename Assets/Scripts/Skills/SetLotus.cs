using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLotus : ISkill
{
    public void Play()
    {
        LotusManager.Instance.Generate(Frog.Instance.transform.position);
    }

    public void Update(float deltaTime) { }
}
