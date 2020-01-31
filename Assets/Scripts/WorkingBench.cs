using UnityEngine;

public class WorkingBench : Interactable
{
    public bool Interact(Item item)
    {
        //TODO validieren und quicktime event
        Destroy(item.gameObject);
        return true;
    }
}
