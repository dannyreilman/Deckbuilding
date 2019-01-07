using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SpellFragment : ScriptableObject
{
    public string description;
    public abstract IEnumerator Play();
}