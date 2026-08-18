using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionsConfig", menuName = "Scriptable Objects/UtilitySystem/ActionsConfig")]
public class ActionsConfig : ScriptableObject
{
    [SerializeReference, SubclassSelector] private List<BaseAIAction> _actions;

    public List<BaseAIAction> GetActions()
    {
        List<BaseAIAction> ret = new();
        foreach (BaseAIAction action in _actions)
        {
            ret.Add(action.CreateShallowCopy());
        }
        return ret;
    }
}
