using InputSystemAgent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace InputSystemAgent
{

    public class InputAgent2
    {
        static InputAgent2 _instance = new InputAgent2();
        static Dictionary<InputAction, Subject<InputAction.CallbackContext>> _inputSubjects;
        //static Dictionary<InputAction, InputAction.CallbackContext> _lastActions;
        private InputAgent2()
        {
            _inputActionAssets = new List<InputActionAsset>();
            foreach (var asset in InputAgentSettings.InputActionAsset)
            {
                foreach (var map in asset.actionMaps)
                {
                    map.actionTriggered += OnAction;
                }
                _inputSubjects = new Dictionary<InputAction, Subject<InputAction.CallbackContext>>();
                _inputActionAssets.Add(asset);

                foreach (var action in asset)
                {
                    _inputSubjects.Add(action, new Subject<InputAction.CallbackContext>());
                    //_lastActions.Add(action, action.);
                }
            }
        }
        List<InputActionAsset> _inputActionAssets;

        public static List<InputActionAsset> InputActionAssets => _instance._inputActionAssets;


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

        public static IDisposable Subscribe(string actionName, Action<InputAction.CallbackContext> action)
        {
            InputAction inputAction = null;
            foreach (var asset in InputActionAssets)
            {
                inputAction = asset.FindAction(actionName);
                if (inputAction != null)
                {
                    return Subscribe(inputAction, action);
                }
            }
            return null;
        }

        public static IDisposable Subscribe(string mapName, string actionName, Action<InputAction.CallbackContext> action)
        {
            InputAction inputAction = null;
            foreach (var asset in InputActionAssets)
            {
                inputAction = asset.FindActionMap(mapName).FindAction(actionName);
                if (inputAction != null)
                {
                    return Subscribe(inputAction, action);
                }
            }
            return null;
        }

        void OnAction(InputAction.CallbackContext context)
        {
            //_lastActions[context.action] = context;
            _inputSubjects[context.action]?.OnNext(context);
        }
    }

}