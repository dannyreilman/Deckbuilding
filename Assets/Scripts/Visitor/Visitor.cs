using UnityEngine;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(menuName="Visitor", fileName="New Visitor")]
public class Visitor : ScriptableObject
{
    public string blurb;
    public Sprite sprite;
    public Buyable[] options;
}
