using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    private Item item;

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
}
