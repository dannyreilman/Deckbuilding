using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class EffectFragment : ScriptableObject
{
    [System.Serializable]
    public enum Type
    {
        Integer, Float, String, Boolean 
    }

    public virtual string GetDescription() {return "";}
    public abstract IEnumerator DoEffectFragment();

    public virtual void AcceptArguments(object[] arguments) {}

    public virtual Type[] GetArgumentTypes()
    {
        Type[] to_return = new Type[0];
        return to_return;
    }
    public virtual string[] GetArgumentNames()
    {
        string[] to_return = new string[0];
        return to_return;
    }

}