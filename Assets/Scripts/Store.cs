using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : Zone
{
	public string name;
	public virtual string GetName()
	{
		return name;
	}
	public Store(string name_in)
	{
		name = name_in;
	}
	public struct StoreEntry
	{
		public string name;
		public List<Card> cards;
		public int priceCoins;
		public int priceAttack;
		public int priceHammers;
		public int priceScience;
	}
	public List<StoreEntry> piles = new List<StoreEntry>();
	public void AddPile(string name, int priceCoins, int priceAttack=0, int priceHammers=0, int priceScience=0)
	{
		StoreEntry e = new StoreEntry();
		e.name = name;
		e.cards = new List<Card>();
		e.priceCoins = priceCoins;
		e.priceAttack = priceAttack;
		e.priceHammers = priceHammers;
		e.priceScience = priceScience;
		piles.Add(e);
	}

	public bool CanBuy(int index)
	{
		if(index >= piles.Count)
		{
			Debug.Log("Bad index!");
			return false;
		}
        GameplayManager gm = GameplayManager.instance;
		return((gm.currentPhase == GameplayManager.Phase.Actions
			|| gm.currentPhase == GameplayManager.Phase.Resources)
			&& gm.coin >= piles[index].priceCoins
			&& gm.attack >= piles[index].priceAttack
			&& gm.hammers >= piles[index].priceHammers
			&& gm.science >= piles[index].priceScience);

	}

    public void Buy(int index)
    {
        GameplayManager gm = GameplayManager.instance;
		if(index < piles.Count
		&& CanBuy(index))
		{
			if(gm.currentPhase == GameplayManager.Phase.Resources)
			{
				gm.AdvancePhase();
			}
			gm.coin -= piles[index].priceCoins;
			gm.attack -= piles[index].priceAttack;
			gm.hammers -= piles[index].priceHammers;
			gm.science -= piles[index].priceScience;
			piles[index].cards[0].MoveTo(gm.discard);
		}
    }

    public void AddCard(Card c)
    {
        foreach(StoreEntry e in piles)
		{
			if(e.cards.Count == 0 || e.name == c.cardname)
			{
				e.cards.Add(c);
				return;
			}
		}
		Debug.Log("Shop added card without pile!");
    }

    public void DropCard(Card c)
    {
        foreach(StoreEntry e in piles)
		{
			if(e.cards.Remove(c))
			{
				if(e.cards.Count == 0)
				{
					piles.Remove(e);
				}
				return;
			}
		}
		Debug.Log("Shop dropped nonexistant card!");
    }

    public int GetCount()
    {
		int count = 0;
        foreach(StoreEntry e in piles)
		{
			count += e.cards.Count;
		}
		return count;
    }
}
