using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public static class PlayerInputExtension
{
    public static bool TryAddListener(this PlayerInput playerInput, string mapName, string actionName, UnityAction<InputAction.CallbackContext> call)
    {
        var actionEvent = playerInput.actionEvents.Where(action =>
        {
            return Regex.IsMatch(action.actionName, $"^{mapName}/{actionName}");
        }).FirstOrDefault();

        if(actionEvent != null )
        {
            actionEvent.AddListener(call);
            return true;
        }
        else
        {
            return false;
        }
    }
}
