using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store
{
	public Resource.Type costType;
	public string name;
	public bool partialBuy;
	public virtual string GetName()
	{
		return name;
	}
	public Store(string name_in, Resource.Type costType_in, bool partialBuy_in = false)
	{
		name = name_in;
		costType = costType_in;
		partialBuy = partialBuy_in;
	}

	[System.Serializable]
	public struct StoreEntrySeed<T> where T : Buyable
	{
		public T toAdd;
		public int count;
		public int price;
	}

	public struct StoreEntry
	{
		public string name;
		public List<Buyable> elements;
		public int price;

		//Used when partial buying is allowed
		public int basePrice;
	}
	public List<StoreEntry> piles = new List<StoreEntry>();
	public void AddPile(string name, int price)
	{
		foreach(StoreEntry pile in piles)
		{
			if(pile.name == name)
				return;
		}
		StoreEntry e = new StoreEntry();
		e.name = name;
		e.elements = new List<Buyable>();
		e.price = price;
		e.basePrice = price;
		piles.Add(e);
	}

	protected bool RightPhase()
	{
		GameplayManager gm = GameplayManager.instance;
		return gm.currentPhase == GameplayManager.Phase.Spending
			|| gm.currentPhase == GameplayManager.Phase.Resources;
	}

	public bool CanBuy(int index)
	{
		if(index >= piles.Count)
		{
			Debug.Log("Bad index!");
			return false;
		}

		GameplayManager gm = GameplayManager.instance;
		switch(costType)
		{
			case Resource.Type.attack:
				return(RightPhase()
					&& gm.attack >= piles[index].price);
			case Resource.Type.coin:
				return(RightPhase()
					&& gm.coin >= piles[index].price);
			case Resource.Type.hammers:
				return(RightPhase()
					&& gm.hammers >= piles[index].price);
			case Resource.Type.science:
				return(RightPhase()
					&& gm.science >= piles[index].price);
			default:
				Debug.Log("Invalid cost type in Store.cs");
				return false;
		}
	}

    public virtual void Buy(int index)
    {
        GameplayManager gm = GameplayManager.instance;
		if(index < piles.Count
		&& CanBuy(index))
		{
			if(gm.currentPhase == GameplayManager.Phase.Resources)
			{
				gm.AdvancePhase();
			}
			switch(costType)
			{
				case Resource.Type.attack:
					gm.attack -= piles[index].price;
					break;
				case Resource.Type.coin:
					gm.coin -= piles[index].price;
					break;
				case Resource.Type.hammers:
					gm.hammers -= piles[index].price;
					break;
				case Resource.Type.science:
					gm.science -= piles[index].price;
					break;
			}
			Buyable bought = piles[index].elements[0];
			StoreEntry entry = piles[index];
			entry.price = piles[index].basePrice;
			piles[index] = entry;
			RemoveElement(bought);
			bought.Buy();
		}
		else if(partialBuy
			 && index < piles.Count
			 && RightPhase())
		{
			switch(costType)
			{
				case Resource.Type.attack:
				{
					if(gm.attack > 0)
					{
						if(gm.currentPhase == GameplayManager.Phase.Resources)
						{
							gm.AdvancePhase();
						}
					}
					StoreEntry entry = piles[index];
					entry.price -= gm.attack;
					piles[index] = entry;
					gm.attack = 0;
					break;
				}
				case Resource.Type.coin:
				{
					if(gm.coin > 0)
					{
						if(gm.currentPhase == GameplayManager.Phase.Resources)
						{
							gm.AdvancePhase();
						}
					}
					StoreEntry entry = piles[index];
					entry.price -= gm.coin;
					piles[index] = entry;
					gm.coin = 0;
					break;
				}
				case Resource.Type.hammers:
				{
					if(gm.hammers > 0)
					{
						if(gm.currentPhase == GameplayManager.Phase.Resources)
						{
							gm.AdvancePhase();
						}
					}
					StoreEntry entry = piles[index];
					entry.price -= gm.hammers;
					piles[index] = entry;
					gm.hammers = 0;
					break;
				}
				case Resource.Type.science:
				{
					if(gm.science > 0)
					{
						if(gm.currentPhase == GameplayManager.Phase.Resources)
						{
							gm.AdvancePhase();
						}
					}
					StoreEntry entry = piles[index];
					entry.price -= gm.science;
					piles[index] = entry;
					gm.science = 0;
					break;
				}
			}
		}
    }

    public virtual void AddElement(Buyable element)
    {
        foreach(StoreEntry e in piles)
		{
			if(e.elements.Count == 0 || e.name == element.GetName())
			{
				e.elements.Add(element);
				return;
			}
		}
		Debug.Log("Shop added element without pile!");
    }

    public void RemoveElement(Buyable element)
    {
        foreach(StoreEntry e in piles)
		{
			if(e.elements.Remove(element))
			{
				if(e.elements.Count == 0)
				{
					piles.Remove(e);
				}
				return;
			}
		}
		Debug.Log("Shop dropped nonexistant element!");
    }

    public int GetCount()
    {
		int count = 0;
        foreach(StoreEntry e in piles)
		{
			count += e.elements.Count;
		}
		return count;
    }
}
