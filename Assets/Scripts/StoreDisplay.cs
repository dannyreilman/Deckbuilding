using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreDisplay : MonoBehaviour, ZoneDisplay
{
    Store toDisplay = null;
    public GameObject entryPrefab;
    public void Display(Zone z)
    {
        toDisplay = (Store)z;
    }

    //Separate function because I might later call this less frequently than once per frame
    void UpdateDisplay()
    {
        while(transform.childCount < toDisplay.piles.Count)
        {
            GameObject instance = Instantiate(entryPrefab,Vector3.zero, Quaternion.identity, transform);
        }

        while(transform.childCount > toDisplay.piles.Count)
        {
            Transform toRemove = transform.GetChild(transform.childCount -1);
            toRemove.SetParent(null);
            Destroy(toRemove.gameObject);
        }

        for(int i = 0; i < toDisplay.piles.Count; ++i)
        {
            transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = toDisplay.piles[i].cards[0].image;
            //TODO: fix this to work with different costs
            transform.GetChild(i).GetChild(1).GetComponent<Text>().text = toDisplay.piles[i].priceCoins.ToString();
            transform.GetChild(i).GetChild(3).GetComponent<Text>().text = toDisplay.piles[i].name;
            transform.GetChild(i).GetChild(4).GetComponent<Text>().text = toDisplay.piles[i].cards.Count.ToString();
            {
                int j = i;
                transform.GetChild(i).GetComponent<ClickBehaviour>().onClick = (()=>toDisplay.Buy(j));
                transform.GetChild(i).GetComponent<ClickBehaviour>().onRightClick = (()=>CardZoom.instance.Show(toDisplay.piles[j].cards[0]));
            }
        }
    }
    void Update()
    {
        if(toDisplay != null)
        {
            UpdateDisplay();
            
        }
    }
}