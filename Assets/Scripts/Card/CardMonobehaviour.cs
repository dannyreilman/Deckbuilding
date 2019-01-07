using UnityEngine;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;

public class CardMonobehaviour : MonoBehaviour
{
     public static CardMonobehaviour instance;
     
     void Awake() {
        instance = this;
     }
}