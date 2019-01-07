using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public const int INIT_CARDS_COUNT = 50;
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
        Visitor,
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
    public CardSet coinsStarting;
    public BlueprintSet hammersStarting;
    public CardSet researchCoinsStarting;
    public BlueprintSet researchBlueprintStarting;
    public EnemySet attackStarting;

    public EnemySet commonEnemies;
    public EnemySet rareEnemies;

    public CardSet commonCards;
    public CardSet rareCards;
    public CardSet superRareCards;

    [HideInInspector]
    public Phase currentPhase;

    public int turnsUntilWave = 5;

    public static GameplayManager instance = null;
    void Awake()
    {
        if(instance == null || instance.Equals(null))
        {
            instance = this;
            hand = new PhysicalPile("hand", GameObject.Find("Hand").transform);
            play = new PhysicalPile("play", GameObject.Find("PlayZone").transform);
            GameObject.Find("Discard").GetComponent<ZoneDisplay>().Display(discard);
            GameObject.Find("Deck").GetComponent<ZoneDisplay>().Display(deck);
            GameObject.Find("CoinsStore").GetComponent<StoreDisplay>().Display(coinsShop);
            GameObject.Find("HammersStore").GetComponent<StoreDisplay>().Display(hammersShop);
            
            GameObject.Find("AttackStore").GetComponent<StoreDisplay>().Display(attackShop);
            GameObject.Find("ScienceStore").GetComponent<StoreDisplay>().Display(scienceShop);

        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        StartCoroutine(StartOfGame());
    }

    public void AutoplayResources()
    {
        StartCoroutine(Autoplay());
    }

    IEnumerator Autoplay()
    {
        if(currentPhase == Phase.Resources)
        {
            for(int i = hand.cards.Count - 1; i >= 0; --i)
            {
                if(hand.cards[i] is Resource && ((Resource)hand.cards[i]).PlayAll())
                {
                    yield return hand.cards[i].OnPlay();
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
            case Phase.Visitor:
                currentPhase = Phase.Spells;
                break;
            case Phase.Spells:
                currentPhase = Phase.Resources;
                break;
            case Phase.Resources:
                currentPhase = Phase.Spending;
                break;
            case Phase.Spending:
                StartCoroutine(EndOfTurn());
                break;
        }
        Debug.Log("Switched to " + currentPhase.ToString());
    }

    public IEnumerator DrawCard()
    {
        InputManager.instance.Animate();
        yield return new WaitForSeconds(0.025f);
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
        InputManager.instance.Play();
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

    public IEnumerator StartOfGame()
    {
        foreach(BaseDeckEntry e in startingDeck)
        {
            for(int i = 0; i < e.copies; ++i)
            {
                e.card.Clone().MoveTo(deck);
            }
        }

        if(coinsStarting != null)
        {
            foreach(Card c in coinsStarting.cards)
            {
                Store.StoreEntry storeEntry = new Store.StoreEntry();
                coinsShop.AddPile(c.cardname, c.baseCost);
                for(int i = 0; i < INIT_CARDS_COUNT; ++i)
                {
                    coinsShop.AddElement(c.Clone());
                }
            }
        }

        if(hammersStarting != null)
        {
            foreach(Blueprint b in hammersStarting.blueprints)
            {
                Store.StoreEntry storeEntry = new Store.StoreEntry();
                hammersShop.AddPile(b.GetName(), b.baseCost);
                hammersShop.AddElement(b.Clone());
            }
        }

        if(attackStarting != null)
        {
            foreach(Enemy e in attackStarting.enemies)
            {
                Store.StoreEntry storeEntry = new Store.StoreEntry();
                attackShop.AddPile(e.GetName(), e.health);
                attackShop.AddElement(e.Clone());
            }
        }

        if(researchCoinsStarting != null)
        {
            foreach(Card c in researchCoinsStarting.cards)
            {
                Store.StoreEntry storeEntry = new Store.StoreEntry();
                scienceShop.AddPile("Research " + c.GetName(), c.researchCost);
                scienceShop.AddElement(new Research(c.Clone()));
            }
        }

        if(researchBlueprintStarting != null)
        {
            foreach(Blueprint b in researchBlueprintStarting.blueprints)
            {
                Store.StoreEntry storeEntry = new Store.StoreEntry();
                scienceShop.AddPile("Research " + b.GetName(), b.researchCost);
                scienceShop.AddElement(new Research(b.Clone()));
            }
        }
        for(int i = 0; i < 4; ++i)
        {
            Card c = ChooseRandomCard();
            if(commonCards.cards.Length + rareCards.cards.Length + superRareCards.cards.Length > 4)
            {
                while(coinsShop.HasPile(c.GetName()))
                {
                    c = ChooseRandomCard();
                }
            }
            Store.StoreEntry storeEntry = new Store.StoreEntry();
            coinsShop.AddPile(c.GetName(), c.baseCost);
            for(int j = 0; j < 5; ++j)
            {
                coinsShop.AddElement(c.Clone());
            }
        }

        health = 100;
        yield return StartOfTurn();
    }

    public IEnumerator StartOfTurn()
    {
        currentPhase = Phase.Visitor;
        AdvancePhase();
        energy = 1;
        coin = 0;
        attack = 0;
        hammers = 0;
        science = 0;

        for(int i = 0; i < 5; ++i)
        {
            yield return DrawCard();
        }

        foreach(Blueprint b in built)
        {
            b.OnStartOfTurn();
        }
    }

    public IEnumerator SpawnWave()
    {
        for(int i = 0; i < 3; ++i)
        {
            //Spawn common enemies
            int chosen = Random.Range(0,commonEnemies.enemies.Length);
            Store.StoreEntry storeEntry = new Store.StoreEntry();
            Enemy e = commonEnemies.enemies[chosen].Clone();
            attackShop.AddPile(e.GetName(), e.health);
            attackShop.AddElement(e.Clone());
        }

        for(int i = 0; i < 2; ++i)
        {
            //Spawn rare enemies
            int chosen = Random.Range(0,rareEnemies.enemies.Length);
            Store.StoreEntry storeEntry = new Store.StoreEntry();
            Enemy e = rareEnemies.enemies[chosen].Clone();
            attackShop.AddPile(e.GetName(), e.health);
            attackShop.AddElement(e.Clone());
        }

        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator EndOfTurn()
    {
        InputManager.instance.Animate();
        DiscardHand();
        DiscardPlay();
        foreach(Enemy e in activeEnemies)
        {
            e.DealAttack();
            yield return new WaitForSeconds(0.10f);
        }
        turnsUntilWave--;
        if(turnsUntilWave <= 0)
        {
            yield return SpawnWave();
            turnsUntilWave = 5;
        }
        yield return new WaitForSeconds(0.25f);
        if(health <= 0)
        {
            Debug.Log("Defeat");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        InputManager.instance.Play();
        yield return StartOfTurn();
    }

    public Card ChooseRandomCard(float commonWeight = 1.0f,
                                 float rareWeight = 0.5f,
                                 float superRareWeight = 0.1f)
    {
        float commonEnd = commonCards.cards.Length * commonWeight;
        float rareEnd = commonEnd +
                        rareCards.cards.Length * rareWeight;
        float superRareEnd = rareEnd +
                             superRareCards.cards.Length * superRareWeight;
        //In case there are no cards at all
        if(superRareEnd == 0)
            return null;
        
        float chosen = Random.Range(0, superRareEnd);

        //Sometimes reroll to simulate exclusivity on the second argument
        while(chosen == superRareEnd)
        {
            chosen = Random.Range(0, superRareEnd);
        }
        
        if(chosen < commonEnd)
        {
            int chosenIndex = Mathf.FloorToInt(chosen / commonWeight);
            return commonCards.cards[chosenIndex].Clone();
        }
        else if(chosen < rareEnd)
        {
            int chosenIndex = Mathf.FloorToInt(chosen - commonEnd / rareWeight);
            return rareCards.cards[chosenIndex].Clone();
        }
        else
        {
            int chosenIndex = Mathf.FloorToInt(chosen - rareEnd / superRareEnd);
            return superRareCards.cards[chosenIndex].Clone();
        }
    }
}
