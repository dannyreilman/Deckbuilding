using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{

    public const int BASIC_CARDS_COUNT = 50;
    public const int INIT_STORE_CARDS_COUNT = 3;
    public ShuffledPile deck = new ShuffledPile("deck");
    public Pile discard = new Pile("discard");
    public Pile destroyedCards = new Pile("destroyed");
    public Store coinsShop = new Store("coins", Store.Type.coin);
    public Store hammersShop = new Store("hammers", Store.Type.hammers, true);
    public List<Blueprint> built = new List<Blueprint>();
    public Store scienceShop = new Store("science", Store.Type.science, true);
    public EnemyStore attackShop = new EnemyStore("enemies", Store.Type.attack);
    public PhysicalPile play;
    public PhysicalPile hand;
    public int energy;
    public int coin;
    public int attack;
    public int hammers;
    public int science;
    [HideInInspector]
    public int health = 100;

    public List<Enemy> activeEnemies = new List<Enemy>();

    [System.Serializable]
    public struct BaseDeckEntry
    {
        public Card card;
        public int copies;
    }

    public BaseDeckEntry[] startingDeck;
    public BuyableSet enemies;
    public Card[] basicCards;
    public BuyableSet cards;
    public BuyableSet blueprints;

    public GameObject visitorPrefab;
    public Visitor testingVisitor;

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

    class VisitorEffect : Effect
    {
        Visitor toSpawn;
        public VisitorEffect(Visitor toSpawn_in)
        {
            toSpawn = toSpawn_in;
        }
        
        public override IEnumerator DoEffect()
        {
            GameObject spawnedInstance = Instantiate(GameplayManager.instance.visitorPrefab, GameplayManager.instance.transform.parent);
            spawnedInstance.GetComponent<ChoicePanel>().Initialize(toSpawn);

            yield return new WaitUntil(()=>spawnedInstance.Equals(null) || spawnedInstance == null);
        }
    }

    public void AutoplayResources()
    {
        StartCoroutine(Autoplay());
    }

    IEnumerator Autoplay()
    {
        for(int i = hand.cards.Count - 1; i >= 0; --i)
        {
            if(hand.cards[i].autoplay)
            {
                yield return hand.cards[i].OnPlayWrapper();
            }
        }
    }

    public IEnumerator DrawCard()
    {
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
    
        foreach(Card c in basicCards)
        {
            Store.StoreEntry storeEntry = new Store.StoreEntry();
            coinsShop.AddPile(c.name, c.buyCost);
            for(int j = 0; j < BASIC_CARDS_COUNT; ++j)
            {
                coinsShop.AddElement(c.Clone());
            }
        }

        BuyableSet uniqueCards = cards.Clone();
        //Don't add cards that we already have
        uniqueCards.Modify((Buyable b) => coinsShop.HasPile(b.name), 0.0f);

        for(int i = 0; i < 4; ++i)
        {
            Card c = (Card)uniqueCards.RandomlyChooseElement();
            if(c != null)
            {
                Store.StoreEntry storeEntry = new Store.StoreEntry();
                coinsShop.AddPile(c.name, c.buyCost);
                for(int j = 0; j < INIT_STORE_CARDS_COUNT; ++j)
                {
                    coinsShop.AddElement(c.Clone());
                }
            }
        }

        BuyableSet uniqueBlueprints = blueprints.Clone();
        uniqueBlueprints.Modify((Buyable b) => hammersShop.HasPile(b.name), 0.0f);
        for(int i = 0; i < 4; ++i)
        {
            Blueprint b = (Blueprint)uniqueBlueprints.RandomlyChooseElement();
            if(b != null)
            {
                Store.StoreEntry storeEntry = new Store.StoreEntry();
                hammersShop.AddPile(b.name, b.buyCost);
                hammersShop.AddElement(b.Clone());
            }
        }

        //TODO: Research

        health = 100;
        yield return InputManager.instance.PlayEffect(new StartOfTurn());
    }

    class StartOfTurn: Effect
    {
        public override IEnumerator DoEffect()
        {
            GameplayManager gm = GameplayManager.instance;
            gm.energy = 1;
            gm.coin = 0;
            gm.attack = 0;
            gm.hammers = 0;
            gm.science = 0;

            for(int i = 0; i < 5; ++i)
            {
                yield return gm.DrawCard();
            }

            foreach(Blueprint b in gm.built)
            {
                b.OnStartOfTurn();
            }

            yield return InputManager.instance.PlayEffect(new VisitorEffect(GameplayManager.instance.testingVisitor));
        }
    }

    public IEnumerator SpawnWave()
    {
        for(int i = 0; i < 3; ++i)
        {
            //Spawn enemies
            Store.StoreEntry storeEntry = new Store.StoreEntry();
            Enemy e = ((Enemy)enemies.RandomlyChooseElement()).Clone();
            attackShop.AddPile(e.name, e.health);
            attackShop.AddElement(e.Clone());
        }

        yield return new WaitForSeconds(0.1f);
    }

    public void EndTurn()
    {
        StartCoroutine(InputManager.instance.PlayEffect(new EndOfTurn()));
    }

    class EndOfTurn: Effect
    {
        public override IEnumerator DoEffect()
        {
            GameplayManager gm = GameplayManager.instance;
            gm.DiscardHand();
            gm.DiscardPlay();
            foreach(Enemy e in gm.activeEnemies)
            {
                e.DealAttack();
                yield return new WaitForSeconds(0.10f);
            }
            gm.turnsUntilWave--;
            if(gm.turnsUntilWave <= 0)
            {
                yield return gm.SpawnWave();
                gm.turnsUntilWave = 5;
            }
            yield return new WaitForSeconds(0.10f);
            if(gm.health <= 0)
            {
                Debug.Log("Defeat");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            yield return InputManager.instance.PlayEffect(new StartOfTurn());
        }
    }

    public void AddCardResearch(Card c)
    {
    }

    public void AddBlueprintResearch(Blueprint b)
    {
    }
}
