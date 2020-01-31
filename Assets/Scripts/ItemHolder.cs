using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private GameObject highlight;

    [SerializeField] private GameObject container;

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
        this.item.transform.parent = this.container.transform;
        this.item.transform.localPosition = new Vector2(0, 0);

        return true;
    }

    public Item GetItem()
    {
        this.item.transform.parent = null;

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
