using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected GameObject highlight;
    
    protected virtual void Awake()
    {
        if(highlight)
            this.highlight.SetActive(false);
    }

    public void EnableHighlight()
    {
        if (highlight)
            this.highlight.SetActive(true);
    }

    public void DisableHighlight()
    {
        if (highlight)
            this.highlight.SetActive(false);
    }
}
