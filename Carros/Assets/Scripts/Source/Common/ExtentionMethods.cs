using System;
using UnityEngine;

public static class ExtensionMethods
{
    public static void SetBoolParameterIfExisting(this Animator _animator, string _parameterName, AnimatorControllerParameterType _type, bool _value)
    {
        if (_animator.HasParameterOfType(_parameterName, _type)) {
            _animator.SetBool(_parameterName, _value);
        }
        return;
    }

    public static bool HasParameterOfType(this Animator _animator, string _parameterName, AnimatorControllerParameterType _parameterType)
    {
        if (_parameterName == null || _parameterName == "") { return false; }
        AnimatorControllerParameter[] parameters = _animator.parameters;
        foreach (AnimatorControllerParameter currParam in parameters)
        {
            if (currParam.type == _parameterType && currParam.name == _parameterName){
                return true;
            }
        }
        return false;
    }
}