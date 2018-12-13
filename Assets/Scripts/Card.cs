using UnityEngine;

public class Card : ScriptableObject
{

    public string cardname;
    public Sprite image;

    [HideInInspector]
    public PhysicalCard p_card = null;
    Zone zone = null;

	public void PlayIfPlayable()
	{
		if(CanPlay())
		{
			OnPlay();
		}
	}

    public Zone GetZone()
    {
        return zone;
    }

    public void MoveTo(Zone z)
    {
        if(ReferenceEquals(z, zone))
        {
            Debug.Log("Moved to same");
        }
        else
        {
            if(zone != null && !(zone.Equals(null)))
                zone.DropCard(this);
            z.AddCard(this);
            zone = z;
        }
    }

    public virtual void OnPlay()
    {
        InputManager.instance.RegisterPlay();
        MoveTo(GameplayManager.instance.play);
    }

    public virtual bool CanPlay()
    {
        return InputManager.instance.currentMode == InputManager.InputMode.Playing
            && InputManager.instance.validZones.Contains(zone);
    }

    protected virtual void HandleClone(Card clone)
    {

    }

    public Card Clone()
    {
        Card c = (Card)this.MemberwiseClone();

        if(p_card != null)
        {
            c.p_card = null;
            PhysicalCardFactory.CreateCard(c);
        }

        HandleClone(c);

        return c;
    }

    public bool SameCard(object other)
    {
        return base.Equals(other);
    }

    public override bool Equals(object other)
    {
        return ReferenceEquals(this, other);
    }
}
