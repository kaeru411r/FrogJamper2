using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using UnityEditor;

namespace InputSystemAgent
{

    public class InputAgent : InputActionSystem.IPlayerActions, InputActionSystem.IUIActions
    {
        static InputAgent _instance = new InputAgent();
        static Dictionary<InputAction, Subject<InputAction.CallbackContext>> _inputSubjects;
        private InputAgent()
        {
            var system = new InputActionSystem();
            system.Player.SetCallbacks(this);
            system.UI.SetCallbacks(this);
            system.Player.Enable();
            system.UI.Enable();
            _inputActionAsset = system.asset;
            _inputSubjects = new Dictionary<InputAction, Subject<InputAction.CallbackContext>>();

            foreach (var action in _inputActionAsset)
            {
                _inputSubjects.Add(action, new Subject<InputAction.CallbackContext>());
            }
        }
        InputActionAsset _inputActionAsset;

        public static InputActionAsset InputActionAsset => _instance._inputActionAsset;


        public static IDisposable Subscribe(InputAction inputAction, Action<InputAction.CallbackContext> action)
        {
            if (_inputSubjects.ContainsKey(inputAction))
            {
                return _inputSubjects[inputAction].Subscribe(action);
            }
            else
            {
                return null;
            }
        }

        public static IDisposable Subscribe(string actionName,  Action<InputAction.CallbackContext> action)
        {
            return Subscribe(InputActionAsset.FindAction(actionName), action);
        }

        public static IDisposable Subscribe(string mapName, string actionName,  Action<InputAction.CallbackContext> action)
        {
            return Subscribe(InputActionAsset.FindActionMap(mapName).FindAction(actionName), action);
        }


        public void OnCancel(InputAction.CallbackContext context)
        {
            CallBack(context);
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            CallBack(context);
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            CallBack(context);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            CallBack(context);
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            CallBack(context);
        }

        public void OnMousePosition(InputAction.CallbackContext context)
        {
            CallBack(context);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            CallBack(context);
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            CallBack(context);
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            CallBack(context);
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            CallBack(context);
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            CallBack(context);
        }

        public void OnSkill1(InputAction.CallbackContext context)
        {
            CallBack(context);
        }

        public void OnSkill2(InputAction.CallbackContext context)
        {
            CallBack(context);
        }

        public void OnSkill3(InputAction.CallbackContext context)
        {
            CallBack(context);
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            CallBack(context);
        }

        public void OnTouch(InputAction.CallbackContext context)
        {
            CallBack(context);
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
            CallBack(context);
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
            CallBack(context);
        }

        void CallBack(InputAction.CallbackContext context)
        {
            _inputSubjects[context.action]?.OnNext(context);
        }

    }

}