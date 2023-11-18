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

    public static class InputAgent2
    {
        static private bool Initialize()
        {
            _inputMaps = new List<InputActionMap> ();
            _inputSubjects = new Dictionary<InputAction, Subject<InputAction.CallbackContext>>();
            _lastActions = new Dictionary<InputAction, InputAction.CallbackContext>();
            foreach (var map in new InputActionSystem().asset.actionMaps)
            {
                _inputMaps.Add (map);
                map.actionTriggered += OnAction;
                map.Enable();
                foreach (var action in map)
                {
                    _inputSubjects.Add(action, new Subject<InputAction.CallbackContext>());
                    _lastActions.Add(action, new InputAction.CallbackContext());
                }
            }
            return true;
        }

        static bool _isInitialized = Initialize();
        static Dictionary<InputAction, Subject<InputAction.CallbackContext>> _inputSubjects;
        static Dictionary<InputAction, InputAction.CallbackContext> _lastActions;
        static List<InputActionMap> _inputMaps;


        public static ReadOnlyArray<InputActionMap> InputMaps => _inputMaps.ToArray();


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
            foreach (var map in InputMaps)
            {
                inputAction = map.FindAction(actionName);
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
            foreach (var map in InputMaps)
            {
                if (map.name == mapName)
                {
                    inputAction = map.FindAction(actionName);
                    if (inputAction != null)
                    {
                        return Subscribe(inputAction, action);
                    }
                }
            }
            return null;
        }



        static void OnAction(InputAction.CallbackContext context)
        {
            _lastActions[context.action] = context;
            _inputSubjects[context.action]?.OnNext(context);
        }
    }

}