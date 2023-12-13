using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private void LateUpdate()
    {
        var center = Field.Instance.Center;
        Camera.main.transform.position = new Vector3(center.x, center.y, Camera.main.transform.position.z);
    }
}
