using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName="BuyableSet", fileName="New Set")]
public class BuyableSet : ScriptableObject
{
    public Buyable[] elements;

    private HashSet<Modifier> modifiers = new HashSet<Modifier>();

    public struct Modifier
    {
        public Predicate<Buyable> chooser;
        public float modifier;
    }

    public void Modify(Predicate<Buyable> which, float modification)
    {
        modifiers.Add(new Modifier() {chooser = which, modifier = modification});
    }

    private float[] calculateWeights()
    {
        float[] weights = new float[elements.Length];
        for(int i = 0; i < elements.Length; ++i)
        {
            weights[i] = Buyable.GetRarityWeight(elements[i].rarity);
            foreach(Modifier mod in modifiers)
            {
                if(mod.chooser(elements[i]))
                    weights[i] *= mod.modifier;
            }
        }
        return weights;
    }

    public Buyable RandomlyChooseElement()
    {
        float[] weights = calculateWeights();

        float sumOfWeights = 0;
        foreach(float weight in weights)
        {
            sumOfWeights += weight;
        }

        //There are no elements
        if(sumOfWeights == 0)
            return null;

        float chosen = UnityEngine.Random.Range(0.0f, sumOfWeights);
        float runningSum = 0;
        for(int i = 0; i < elements.Length; ++i)
        {
            if(chosen < runningSum)
            {
                return elements[i];
            }
            else
            {
                runningSum += weights[i];
            }
        }

        //Default to the last element, also will occur if chosen is in the last partition
        return elements[elements.Length - 1];
    }

    //Returns a list of all elements with nonzero weights
    public List<Buyable> GetElements()
    {
        List<Buyable> elementsToReturn = new List<Buyable>();
        float[] weights = calculateWeights();
        for(int i = 0; i < elements.Length; ++i)
        {
            if(weights[i] != 0)
            {
                elementsToReturn.Add(elements[i]);
            }
        }
        return elementsToReturn;
    }

    public BuyableSet Clone()
    {
        BuyableSet other = ScriptableObject.CreateInstance("BuyableSet") as BuyableSet;
        other.elements = new Buyable[elements.Length];
        for(int i = 0; i < elements.Length; ++i)
        {
            //TODO: Maybe can't do this
            other.elements[i] = elements[i];
        }
        other.modifiers = new HashSet<Modifier>(modifiers);

        return other;
    }

    public static BuyableSet operator+ (BuyableSet a, BuyableSet b)
    {
        BuyableSet result = ScriptableObject.CreateInstance("BuyableSet") as BuyableSet;
        result.elements = new Buyable[a.elements.Length + b.elements.Length];
        a.elements.CopyTo(result.elements, 0);
        b.elements.CopyTo(result.elements, a.elements.Length);
        result.modifiers = new HashSet<Modifier>(a.modifiers.Concat(b.modifiers).ToList());
        return result;
    }
}
