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
        private InputAgent2()
        {
            _inputActionAssets = new List<InputActionAsset>();
            foreach (var asset in InputAgentSettings.InputActionAsset)
            {
                asset.actionMaps[0].actionTriggered += OnAction;
                _inputSubjects = new Dictionary<InputAction, Subject<InputAction.CallbackContext>>();
                _inputActionAssets.Add(asset);

                foreach (var action in asset)
                {
                    _inputSubjects.Add(action, new Subject<InputAction.CallbackContext>());
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
            _inputSubjects[context.action]?.OnNext(context);
        }


        [CreateAssetMenu(menuName = "ScriptableObjects/InputAgentSettings")]
        class InputAgentSettings : ScriptableObject
        {
            static Dictionary<InputActionAsset, int> _inputActionAssets = new Dictionary<InputActionAsset, int>();

            public static ReadOnlyArray<InputActionAsset> InputActionAsset => _inputActionAssets.Keys.ToArray();


            public InputActionAsset[] Actions = new InputActionAsset[0];

            private void OnEnable()
            {
                Add();
            }

            void Add()
            {
                foreach (var inputActionAsset in Actions)
                {
                    if (inputActionAsset == null) { continue; }
                    if (_inputActionAssets.ContainsKey(inputActionAsset))
                    {
                        _inputActionAssets[inputActionAsset]++;
                    }
                    else
                    {
                        _inputActionAssets.Add(inputActionAsset, 1);
                    }
                }
            }

        }




    }

}