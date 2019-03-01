using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class InputManager : MonoBehaviour
{
    public static InputManager instance = null;
    public enum InputMode
    {
        None,
        Dragging,
        Selecting,
        Playing
    }

    class InputState
    {
        public InputMode mode;
        public int stepsLeft;
        public bool upTo;
        public HashSet<Zone> validZones = new HashSet<Zone>();
        public List<Card> collectedCards = new List<Card>();
        public FinishInput onFinish;

        //Saves the effect executing while the input was started.
        //If Null, input was started without any effects
        //Check against effect stack to see if input is valid
        public Effect spawningEffect;
    }

    public delegate void FinishInput(List<Card> collected);

    Stack<Effect> effectStack = new Stack<Effect>();
    Stack<InputState> stateStack = new Stack<InputState>();

    public InputMode currentMode
    {
        get
        {
            if(stateStack.Count != 0)
                return stateStack.Peek().mode;
            else
                return InputMode.None;
        }
    }

    public HashSet<Zone> currentValidZones
    {
        get
        {
            if(stateStack.Count != 0)
                return stateStack.Peek().validZones;
            else
                return new HashSet<Zone>();
        }
    }

    public bool currentUpTo
    {
        get
        {
            if(stateStack.Count != 0)
                return stateStack.Peek().upTo;
            else
                return false;
        }
    }

    public int currentStepsLeft
    {
        get
        {
            if(stateStack.Count != 0)
                return stateStack.Peek().stepsLeft;
            else
                return 0;
        }
    }

    [HideInInspector]
    public bool doingEffect
    {
        get
        {
            return effectStack.Count != 0;
        }
    }

    public bool inputValid
    {
        get
        {
            if(stateStack.Count == 0)
                return false;
            
            if(!doingEffect)
                return true;
            
            //Don't allow input during an effect that didn't ask for the input
            return stateStack.Peek().spawningEffect == effectStack.Peek();
        }
    }

    //Trigger a playing input (standard input)
    //if zones is null, defaults to hand, if howMany is -1, defaults to infinite
    public void Play(Zone[] zones = null, int howMany = -1)
    {
        AddNewInput(InputMode.Playing, zones, howMany);
    }

    public void Select(Zone[] zones = null, int howMany = -1, FinishInput onFinish_in = null, bool upTo_in = false)
    {
        AddNewInput(InputMode.Selecting, zones, howMany, onFinish_in, upTo_in);
    }

    void AddNewInput(InputMode toAdd, Zone[] zones = null, int howMany = -1, FinishInput onFinish_in = null, bool upTo_in = false)
    {
        if(zones == null)
        {
            zones = new Zone[]{GameplayManager.instance.hand};
        }
        int totalPossible = 0;
        foreach(Zone z in zones)
        {
            totalPossible += z.GetCount();
        }
        InputState newState = new InputState();
        newState.validZones = new HashSet<Zone>(zones);
        newState.stepsLeft = howMany;
        newState.mode = toAdd;
        newState.onFinish = onFinish_in;
        newState.upTo = upTo_in;
        stateStack.Push(newState);

        foreach(PhysicalCard c in PhysicalCard.interactableCards)
        {
            UpdatePCard(c);
        }
    }

    void PopInputState()
    {
        if(stateStack.Peek().onFinish != null)
        {
            stateStack.Peek().onFinish(stateStack.Peek().collectedCards);
        }

        switch(stateStack.Peek().mode)
        {
            case InputMode.Selecting:
                foreach(Card selected in stateStack.Peek().collectedCards)
                {
                    if(selected.p_card != null)
                    {
                        selected.p_card.Deselect();
                    }
                }
                break;
            default:
            //Do nothing
                break;
        }

        stateStack.Pop();
        if(stateStack.Count == 0)
        {
            //Default to base play
            Play();
        }
    }

    public void FinishSelecting()
    {
        if(stateStack.Peek().upTo || stateStack.Peek().stepsLeft == 0)
        {
            PopInputState();
        }
    }


    public void SelectCard(Card c)
    {
        if(stateStack.Peek().collectedCards.Contains(c))
        {
            stateStack.Peek().collectedCards.Remove(c);
            c.p_card.Deselect();
            ++stateStack.Peek().stepsLeft;
        }
        else
        {
            if(stateStack.Peek().stepsLeft != 0)
            {
                stateStack.Peek().collectedCards.Add(c);
                c.p_card.Select();
                if(stateStack.Peek().stepsLeft > 0)
                {
                    --stateStack.Peek().stepsLeft;
                }
            }
        }
    }

    public void DeselectCard(Card c)
    {
        stateStack.Peek().collectedCards.Remove(c);
    }

    public void ClearSelection()
    {
        stateStack.Peek().collectedCards.Clear();
    }

    public void UpdatePCard(PhysicalCard p_card)
    {
        switch(currentMode)
        {
        case InputMode.Dragging:
            Debug.Log("NOT IMPLEMENTED UPDATEPCARD");
        break;
        case InputMode.Selecting:
            {
                Card cardToSelect = p_card.card;
                p_card.attachedClick.onClick = (() => {if(inputValid)SelectCard(cardToSelect);});
                p_card.attachedClick.onClick += ZoomDisplay.instance.Hide;
            }
        break;
        case InputMode.Playing:
            p_card.attachedClick.onClick = (() => {if(inputValid)p_card.card.PlayIfPlayable();});
            p_card.attachedClick.onClick += ZoomDisplay.instance.Hide;
        break;
        }
    }

    public void RegisterPlay()
    {
        InputModeDisplay.instance.CloseConfirmation();
        if(stateStack.Peek().stepsLeft != -1)
        {
            --stateStack.Peek().stepsLeft;
            if(stateStack.Peek().stepsLeft == 0)
            {
                PopInputState();
            }
        }
    }

    public void PushEffect(Effect e)
    {
        effectStack.Push(e);
    }

    public IEnumerator PlayEffect(Effect e)
    {
        effectStack.Push(e);
        yield return e.DoEffect();
        PopEffect(e);
    }

    public void PopEffect(Effect e)
    {
        Assert.AreEqual(effectStack.Pop(), e);
    }

    void Awake()
    {
        if(instance == null || instance.Equals(null))
        {
            instance = this;
            Play();
        }
        else
        {
            Destroy(this);
        }
    }
}
