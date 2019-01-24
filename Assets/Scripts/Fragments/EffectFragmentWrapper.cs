using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class EffectFragmentWrapper: ISerializationCallbackReceiver
{
    public EffectFragment fragment;
    public System.Object[] arguments;

    //for serialization (Unity doesn't know what do do with System.Object)

    [System.Serializable]
    int[] int_args;
    float[] float_args;
    string[] string_args;
    bool[] bool_args;


    public void GiveArguments()
    {
        fragment.AcceptArguments(arguments);
    }

    public IEnumerator DoEffect()
    {
        yield return DoEffect();
    }

    public string GetDescription()
    {
        return fragment.GetDescription();
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        if(fragment != null)
        {
            EffectFragment.Type[] types = fragment.GetArgumentTypes();
            if(arguments == null || arguments.Length != types.Length)
            {
                arguments = new System.Object[types.Length];
                return;
            }
            int_args = new int[types.Length];
            float_args = new float[types.Length];
            string_args = new string[types.Length];
            bool_args = new bool[types.Length];

            for(int i = 0; i < types.Length; ++i)
            {
                switch(types[i])
                {
                    case EffectFragment.Type.Integer:
                        var temp = (int)arguments[i];
                        int_args[i] = temp;
                        break;
                    case EffectFragment.Type.Float:
                        float_args[i] = (float)arguments[i];
                        break;
                    case EffectFragment.Type.String:
                        string_args[i] = (string)arguments[i];
                        break;
                    case EffectFragment.Type.Boolean:
                        bool_args[i] = (bool)arguments[i];
                        break;
                }
            }
        }
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        if(fragment != null)
        {
            EffectFragment.Type[] types = fragment.GetArgumentTypes();
            arguments = new System.Object[types.Length];
            if(int_args == null || int_args.Length != types.Length)
                int_args = new int[types.Length];
            if(float_args == null || float_args.Length != types.Length)
                float_args = new float[types.Length];          
            if(string_args == null || string_args.Length != types.Length)
                string_args = new string[types.Length];        
            if(bool_args == null || bool_args.Length != types.Length)
                bool_args = new bool[types.Length];

            for(int i = 0; i < types.Length; ++i)
            {
                switch(types[i])
                {
                    case EffectFragment.Type.Integer:
                        arguments[i] = (object)int_args[i];
                        break;
                    case EffectFragment.Type.Float:
                        arguments[i] = (object)float_args[i];
                        break;
                    case EffectFragment.Type.String:
                        arguments[i] = (object)string_args[i];
                        break;
                    case EffectFragment.Type.Boolean:
                        arguments[i] = (object)bool_args[i];
                        break;
                }
            }
        }
    }
}