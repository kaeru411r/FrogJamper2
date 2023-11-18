
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Linq;

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