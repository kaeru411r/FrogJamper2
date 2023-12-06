using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SceneViewSetter : MonoBehaviour
{
    [Tooltip("‘ÎÛ‚Æ‚·‚éƒJƒƒ‰")]
    [SerializeField] private Camera targetCamera;
    [Tooltip("–Ú“I‰ğ‘œ“x")]
    [SerializeField] private Vector2 aspectVec;


    void Update()
    {
        if (targetCamera == null) { return; }
        var screenAspect = Screen.width / (float)Screen.height;
        var targetAspect = aspectVec.x / aspectVec.y;

        var magRate = targetAspect / screenAspect;

        var viewportRect = new Rect(0, 0, 1, 1);

        if (magRate < 1)
        {
            viewportRect.width = magRate;
            viewportRect.x = 0.5f - viewportRect.width * 0.5f;
        }
        else
        {
            viewportRect.height = 1 / magRate;
            viewportRect.y = 0.5f - viewportRect.height * 0.5f;
        }

        targetCamera.rect = viewportRect;
    }
}