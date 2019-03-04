using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyableDisplayer : MonoBehaviour
{
    public Image image;
    public Text title;
    public ClickBehaviour clickBehaviour;

    ChoicePanel parent;
    void Awake()
    {
        parent = GetComponentInParent<ChoicePanel>();
    }

	public void Initialize(Buyable toDisplay)
    {
        if(image != null)
            image.sprite = toDisplay.image;
        if(title != null)
            title.text = toDisplay.name;
        if(clickBehaviour!=null)
        {
            clickBehaviour.onClick = (()=>StartCoroutine(ClickMethod(toDisplay)));
            clickBehaviour.onRightClick = ()=>ZoomDisplay.instance.Show(toDisplay);
        }
	}

	public IEnumerator ClickMethod(Buyable toDisplay)
	{
        parent.Hide();
        yield return toDisplay.Buy();
        parent.Destroy();
	}
}
