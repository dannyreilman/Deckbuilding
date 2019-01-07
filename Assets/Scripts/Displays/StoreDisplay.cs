using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreDisplay : MonoBehaviour
{
    Store toDisplay = null;
    public GameObject entryPrefab;
    public void Display(Store s)
    {
        toDisplay = s;
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
            transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = toDisplay.piles[i].elements[0].GetDisplay();
            transform.GetChild(i).GetChild(1).GetComponent<Text>().text = toDisplay.piles[i].price.ToString();
            transform.GetChild(i).GetChild(3).GetComponent<Text>().text = toDisplay.piles[i].name;
            transform.GetChild(i).GetChild(4).GetComponent<Text>().text = toDisplay.piles[i].elements.Count.ToString();
            {
                int j = i;
                transform.GetChild(i).GetComponent<ClickBehaviour>().onClick = (()=>toDisplay.Buy(j));
                transform.GetChild(i).GetComponent<ClickBehaviour>().onRightClick = ()=>ZoomDisplay.instance.Show(toDisplay.piles[j].elements[0]);
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