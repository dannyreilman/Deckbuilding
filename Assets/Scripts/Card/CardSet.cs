using UnityEngine;

[CreateAssetMenu(menuName="CardSets/CardSet", fileName="New Set")]
public class CardSet : ScriptableObject
{
    public Card[] cards;
}
