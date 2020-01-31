using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private GameObject highlight;

    private Item item;

    private void Awake()
    {
        this.highlight.SetActive(false);
    }

    private bool HasItem()
    {
        return this.item;
    }


    public bool PutItem(Item item)
    {
        if(this.item)
        {
            return false;
        }

        this.item = item;

        return true;
    }

    public Item GetItem()
    {
        return this.item;
    }

    public void EnableHighlight()
    {
        this.highlight.SetActive(true);
    }

    public void DisableHighlight()
    {
        this.highlight.SetActive(false);
    }
}
