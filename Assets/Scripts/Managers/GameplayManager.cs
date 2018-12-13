using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public ShuffledPile deck = new ShuffledPile();
    public Pile discard = new Pile("discard");
    public Pile destroyedCards = new Pile("destroyed");
    public Store coinsShop = new Store("coins");
    public PhysicalPile play;
    public PhysicalPile hand;
    public int energy;
    public int coin;
    public int attack;
    public int hammers;
    public int science;

    public enum Phase
    {
        Spells,
        Resources,
        Actions
    }

    [System.Serializable]
    public struct BaseDeckEntry
    {
        public Card card;
        public int copies;
    }
    [System.Serializable]
    public struct BaseShopEntry
    {
        public Card card;
        public int copies;
        public int cost;
    }

    public BaseDeckEntry[] startingDeck;
    public BaseShopEntry[] startingShop;

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
            StartOfGame();
        }
        else
        {
            Destroy(this);
        }
    }

    void Update()
    {
        if(currentPhase != Phase.Actions)
        {
            if(currentPhase == Phase.Resources && Input.GetKeyDown(KeyCode.A))
            {
                for(int i = hand.cards.Count - 1; i >= 0; --i)
                {
                    if(hand.cards[i] is Resource && ((Resource)hand.cards[i]).PlayAll())
                    {
                        hand.cards[i].OnPlay();
                    }
                }
            }

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
        else
        {
            if(!Input.GetKeyDown(KeyCode.Space) && InputManager.instance.currentMode == InputManager.InputMode.Playing)
            {
                bool anythingToDo = false;
                for(int i = 0; i < coinsShop.piles.Count; ++i)
                {
                    if(coinsShop.CanBuy(i))
                    {
                        anythingToDo = true;
                    }
                }

                if(!anythingToDo)
                {
                    AdvancePhase();
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AdvancePhase();
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
                currentPhase = Phase.Actions;
                break;
            case Phase.Actions:
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
            Debug.Log("Drew a card");
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
        foreach(BaseShopEntry e in startingShop)
        {
            Store.StoreEntry storeEntry = new Store.StoreEntry();
            coinsShop.AddPile(e.card.cardname, e.cost);
            for(int i = 0; i < e.copies; ++i)
            {
                e.card.Clone().MoveTo(coinsShop);
            }
        }
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
    }

    public void EndOfTurn()
    {
        DiscardHand();
        DiscardPlay();
    }
}
