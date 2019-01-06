using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public ShuffledPile deck = new ShuffledPile("deck");
    public Pile discard = new Pile("discard");
    public Pile destroyedCards = new Pile("destroyed");
    public Store coinsShop = new Store("coins", Resource.Type.coin);
    public Store hammersShop = new Store("hammers", Resource.Type.hammers, true);
    public List<Blueprint> built = new List<Blueprint>();
    public Store scienceShop = new Store("science", Resource.Type.science, true);
    public EnemyStore attackShop = new EnemyStore("enemies", Resource.Type.attack);
    public PhysicalPile play;
    public PhysicalPile hand;
    public int energy;
    public int coin;
    public int attack;
    public int hammers;
    public int science;
    public int health;

    public List<Enemy> activeEnemies = new List<Enemy>();

    public enum Phase
    {
        Spells,
        Resources,
        Spending
    }

    [System.Serializable]
    public struct BaseDeckEntry
    {
        public Card card;
        public int copies;
    }

    public BaseDeckEntry[] startingDeck;
    public CardStoreSet coinsStarting;
    public BlueprintSet hammersStarting;
    public CardStoreSet researchCoinsStarting;
    public BlueprintSet researchBlueprintStarting;
    public EnemySet attackStarting;

    [HideInInspector]
    public Phase currentPhase;

    public static GameplayManager instance = null;
    void Awake()
    {
        if(instance == null || instance.Equals(null))
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            hand = new PhysicalPile("hand", GameObject.Find("Hand").transform);
            play = new PhysicalPile("play", GameObject.Find("PlayZone").transform);
            GameObject.Find("Discard").GetComponent<ZoneDisplay>().Display(discard);
            GameObject.Find("Deck").GetComponent<ZoneDisplay>().Display(deck);
            GameObject.Find("CoinsStore").GetComponent<StoreDisplay>().Display(coinsShop);
            GameObject.Find("HammersStore").GetComponent<StoreDisplay>().Display(hammersShop);
            
            GameObject.Find("AttackStore").GetComponent<StoreDisplay>().Display(attackShop);
            GameObject.Find("ScienceStore").GetComponent<StoreDisplay>().Display(scienceShop);

            StartOfGame();
        }
        else
        {
            Destroy(this);
        }
    }

    public void AutoplayResources()
    {
        if(currentPhase == Phase.Resources)
        {
            for(int i = hand.cards.Count - 1; i >= 0; --i)
            {
                if(hand.cards[i] is Resource && ((Resource)hand.cards[i]).PlayAll())
                {
                    hand.cards[i].OnPlay();
                }
            }
        }
        
    }

    void Update()
    {
        if(currentPhase != Phase.Spending)
        {
            if(!Input.GetKeyDown(KeyCode.Space) && InputManager.instance.currentMode == InputManager.InputMode.Playing)
            {
                bool autoPass = true;
                foreach(Card c in hand.cards)
                {
                    if(c.CanPlay())
                        autoPass = false;
                }
                if(autoPass)
                {
                    AdvancePhase();
                }
            }
        }
    }

    public void AdvancePhase()
    {
        switch(currentPhase)
        {
            case Phase.Spells:
                currentPhase = Phase.Resources;
                break;
            case Phase.Resources:
                currentPhase = Phase.Spending;
                break;
            case Phase.Spending:
                EndOfTurn();
                StartOfTurn();
                break;
        }
        Debug.Log("Switched to " + currentPhase.ToString());
    }

    public void DrawCard()
    {
        if(deck.cards.Count == 0)
        {
            for(int i = discard.cards.Count - 1; i >= 0; --i)
            {
                discard.cards[i].MoveTo(deck);
            }
        }

        //Second check for if you draw the whole deck
        if(deck.cards.Count > 0)
        {
            deck.cards[0].MoveTo(hand);
        }
        else
        {
            Debug.Log("Deck empty");
        }
    }

    public void DiscardHand()
    {
        for(int i = hand.cards.Count - 1; i >= 0; --i)
        {
            hand.cards[i].MoveTo(discard);
        }
    }

    public void DiscardPlay()
    {
        for(int i = play.cards.Count - 1; i >= 0; --i)
        {
            play.cards[i].MoveTo(discard);
        }
    }

    public void StartOfGame()
    {
        foreach(BaseDeckEntry e in startingDeck)
        {
            for(int i = 0; i < e.copies; ++i)
            {
                e.card.Clone().MoveTo(deck);
            }
        }

        foreach(Card c in coinsStarting.cardSet.cards)
        {
            Store.StoreEntry storeEntry = new Store.StoreEntry();
            coinsShop.AddPile(c.cardname, c.baseCost);
            for(int i = 0; i < coinsStarting.countEach; ++i)
            {
                coinsShop.AddElement(c.Clone());
            }
        }

        foreach(Blueprint b in hammersStarting.blueprints)
        {
            Store.StoreEntry storeEntry = new Store.StoreEntry();
            hammersShop.AddPile(b.GetName(), b.baseCost);
            hammersShop.AddElement(b.Clone());
        }

        foreach(Enemy e in attackStarting.enemies)
        {
            Store.StoreEntry storeEntry = new Store.StoreEntry();
            attackShop.AddPile(e.GetName(), e.health);
            attackShop.AddElement(e.Clone());
        }

        foreach(Card c in researchCoinsStarting.cardSet.cards)
        {
            Store.StoreEntry storeEntry = new Store.StoreEntry();
            scienceShop.AddPile("Research " + c.GetName(), c.researchCost);
            scienceShop.AddElement(new Research(c.Clone()));
        }

        foreach(Blueprint b in researchBlueprintStarting.blueprints)
        {
            Store.StoreEntry storeEntry = new Store.StoreEntry();
            scienceShop.AddPile("Research " + b.GetName(), b.researchCost);
            scienceShop.AddElement(new Research(b.Clone()));
        }

        health = 100;
        StartOfTurn();
    }

    public void StartOfTurn()
    {
        currentPhase = Phase.Spells;
        energy = 1;
        coin = 0;
        attack = 0;
        hammers = 0;
        science = 0;

        for(int i = 0; i < 5; ++i)
        {
            DrawCard();
        }

        foreach(Blueprint b in built)
        {
            b.OnStartOfTurn();
        }
    }

    public void EndOfTurn()
    {
        DiscardHand();
        DiscardPlay();
        foreach(Enemy e in activeEnemies)
        {
            e.DealAttack();
        }
    }
}
