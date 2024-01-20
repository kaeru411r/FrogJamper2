using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Yield
{
    public static IEnumerator WaitForSeconds(float time)
    {
        while (time > 0)
        {
            yield return null;
            time -= Time.deltaTime;
        }
    }
}
