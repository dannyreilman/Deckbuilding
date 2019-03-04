using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Change add pile/add element syntax and add a pile height (use Buyable.buyCost)
public class Store
{
	public enum Type
	{
		energy,
		coin,
		hammers,
		science,
		attack
	}

	public Store.Type costType;
	public string name;
	public bool partialBuy;
	public virtual string GetName()
	{
		return name;
	}
	public Store(string name_in, Store.Type costType_in, bool partialBuy_in = false)
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
	public bool HasPile(string name)
	{
		foreach(StoreEntry pile in piles)
		{
			if(pile.name == name)
				return true;
		}
		return false;
	}
	public bool AnyBuyable()
	{
		for(int i = 0; i < piles.Count; ++i)
		{
			if(piles[i].price > 0 && CanBuy(i))
				return true;
		}
		return false;
	}
	public void AddPile(string name, int price)
	{
		if(HasPile(name))
			return;
		StoreEntry e = new StoreEntry();
		e.name = name;
		e.elements = new List<Buyable>();
		e.price = price;
		e.basePrice = price;
		piles.Add(e);
	}
	public bool CanAfford(int index)
	{
		GameplayManager gm = GameplayManager.instance;
		switch(costType)
		{
			case Store.Type.attack:
				return(gm.attack >= piles[index].price);
			case Store.Type.coin:
				return(gm.coin >= piles[index].price);
			case Store.Type.hammers:
				return(gm.hammers >= piles[index].price);
			case Store.Type.science:
				return(gm.science >= piles[index].price);
			default:
				Debug.Log("Invalid cost type in Store.cs");
				return false;
		}
	}

	public bool CanBuy(int index)
	{
		if(index >= piles.Count)
		{
			Debug.Log("Bad index!");
			return false;
		}

		//Can't buy if you can't make inputs
		if(!InputManager.instance.inputValid)
		{
			return false;
		}

		return CanAfford(index);
	}

    public virtual void Buy(int index)
    {
        InputModeDisplay.instance.CloseConfirmation();
        GameplayManager gm = GameplayManager.instance;
		if(index < piles.Count
		&& CanBuy(index))
		{
			switch(costType)
			{
				case Store.Type.attack:
					gm.attack -= piles[index].price;
					break;
				case Store.Type.coin:
					gm.coin -= piles[index].price;
					break;
				case Store.Type.hammers:
					gm.hammers -= piles[index].price;
					break;
				case Store.Type.science:
					gm.science -= piles[index].price;
					break;
			}
			Buyable bought = piles[index].elements[0];
			StoreEntry entry = piles[index];
			entry.price = piles[index].basePrice;
			piles[index] = entry;
			RemoveElement(bought);
			GameplayManager.instance.StartCoroutine(bought.Buy());
		}
		else if(partialBuy
			 && index < piles.Count)
		{
			switch(costType)
			{
				case Store.Type.attack:
				{
					StoreEntry entry = piles[index];
					entry.price -= gm.attack;
					piles[index] = entry;
					gm.attack = 0;
					break;
				}
				case Store.Type.coin:
				{
					StoreEntry entry = piles[index];
					entry.price -= gm.coin;
					piles[index] = entry;
					gm.coin = 0;
					break;
				}
				case Store.Type.hammers:
				{
					StoreEntry entry = piles[index];
					entry.price -= gm.hammers;
					piles[index] = entry;
					gm.hammers = 0;
					break;
				}
				case Store.Type.science:
				{
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
			if(e.elements.Count == 0 || e.name == element.name)
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
