using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance = null;
    public enum InputMode
    {
        Dragging,
        Selecting,
        Playing
    }
    public InputMode currentMode;
    public int stepsLeft;
    public HashSet<Zone> validZones = new HashSet<Zone>();
    public List<Card> selectedCards = new List<Card>();
    public delegate void FinishSelect(List<Card> selected);
    FinishSelect onFinishSelect;

    //Trigger a playing input (standard input)
    //if zones is null, defaults to hand, if howMany is -1, defaults to infinite
    public void Play(Zone[] zones = null, int howMany = -1)
    {
        if(zones == null)
        {
            zones = new Zone[]{GameplayManager.instance.hand};
        }
        validZones = new HashSet<Zone>(zones);
        stepsLeft = howMany;
        currentMode = InputMode.Playing;
        foreach(PhysicalCard c in PhysicalCard.interactableCards)
        {
            UpdatePCard(c);
        }
    }

    public void Select(Zone[] zones = null, int howMany = -1, FinishSelect onFinish_in = null)
    {
        if(zones == null)
        {
            zones = new Zone[]{GameplayManager.instance.hand};
        }
        stepsLeft = howMany;
        currentMode = InputMode.Selecting;
        onFinishSelect = onFinish_in;
        selectedCards = new List<Card>();
        foreach(PhysicalCard c in PhysicalCard.interactableCards)
        {
            UpdatePCard(c);
        }
    }

    public void SelectCard(Card c)
    {
        if(selectedCards.Contains(c))
        {
            selectedCards.Remove(c);
            ++stepsLeft;
        }
        else
        {
            selectedCards.Add(c);
            if(stepsLeft > 0)
            {
                --stepsLeft;
                if(stepsLeft == 0)
                {
                    if(onFinishSelect != null)
                    {
                        onFinishSelect(selectedCards);
                    }

                    selectedCards.Clear();
                    Play();
                }
            }
        }
        Debug.Log(selectedCards.Count);
    }

    public void DeselectCard(Card c)
    {
        selectedCards.Remove(c);
    }

    public void ClearSelection()
    {
        selectedCards.Clear();
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
                p_card.attachedClick.onClick = (() => SelectCard(cardToSelect));
            }
        break;
        case InputMode.Playing:
            p_card.attachedClick.onClick = p_card.card.PlayIfPlayable;
        break;
        }
    }

    public void RegisterPlay()
    {
        if(stepsLeft != -1)
        {
            --stepsLeft;
            if(stepsLeft == 0)
            {
                //Return to default
                Play();
            }
        }
    }

    void Start()
    {
        if(instance == null || instance.Equals(null))
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Play();
        }
        else
        {
            Destroy(this);
        }
    }




}
