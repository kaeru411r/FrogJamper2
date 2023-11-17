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

    /// <summary>
    /// Action�Ƀ��\�b�h��o�^����
    /// </summary>
    /// <param name="playerInput"></param>
    /// <param name="map"></param>
    /// <param name="action"></param>
    /// <param name="call"></param>
    /// <returns></returns>
    public static void AddListener(this PlayerInput playerInput, string map, string action, UnityAction<InputAction.CallbackContext> call)
    {
        var actionEvent = playerInput.actionEvents.Where(item =>
        {
            return Regex.IsMatch(item.actionName, $"^{map}/{action}");
        }).FirstOrDefault();

        if(actionEvent != null )
        {
            actionEvent.AddListener(call);
        }
        else
        {
            throw (new System.Exception("�A�N�V������������܂���"));
        }
    }
    
    /// <summary>
    /// Action�Ƀ��\�b�h��o�^����
    /// </summary>
    /// <param name="playerInput"></param>
    /// <param name="map"></param>
    /// <param name="action"></param>
    /// <param name="call"></param>
    /// <returns></returns>
    public static void AddListener(this PlayerInput playerInput, string id, UnityAction<InputAction.CallbackContext> call)
    {
        var actionEvent = playerInput.actionEvents.Where(item =>
        {
            return Regex.IsMatch(item.actionId, id);
        }).FirstOrDefault();

        if(actionEvent != null )
        {
            actionEvent.AddListener(call);
        }
        else
        {
            throw (new System.Exception("�A�N�V������������܂���"));
        }
    }

    /// <summary>
    /// Action���烁�\�b�h�����O����
    /// </summary>
    /// <param name="playerInput"></param>
    /// <param name="map"></param>
    /// <param name="action"></param>
    /// <param name="call"></param>
    public static void RemoveListenr(this PlayerInput playerInput, string map, string action, UnityAction<InputAction.CallbackContext> call)
    {
        playerInput.actionEvents.Where(item =>
        {
            return Regex.IsMatch(item.actionName, $"^{map}/{action}");
        }).FirstOrDefault().RemoveListener(call);
    }
}
