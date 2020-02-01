using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : Interactable
{
    [SerializeField] private GameObject container;
    [SerializeField] private bool freezesItem = false;

    private Item item;


    public bool HasItem()
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
        this.item.transform.parent = this.container.transform;
        this.item.transform.localPosition = new Vector2(0, 0);

        if(this.freezesItem)
        {
            this.item.Freeze();
        }

        return true;
    }

    public Item GetItem()
    {
        this.item.transform.parent = null;


        this.item.Unfreeze();

        Item itm = this.item;
        this.item = null;
        
        return itm;
    }
}
