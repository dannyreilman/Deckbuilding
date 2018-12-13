using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalCardFactory : MonoBehaviour
{
    public static PhysicalCardFactory instance = null;
    public GameObject prefab;

    public static PhysicalCard CreateCard(Card c)
    {
        GameObject obj = Instantiate(instance.prefab);
        PhysicalCard pcard = obj.GetComponent<PhysicalCard>();
        pcard.Associate(c);
        return pcard;
    }
    
    void Awake()
    {
        if(instance == null || instance.Equals(null))
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }


}
