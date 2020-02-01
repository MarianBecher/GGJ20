using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class QTEIndicator : MonoBehaviour
{
    private Image _img;

    private void Awake()
    {
        _img = GetComponent<Image>();
    }

    public void Initialze(Sprite sprite)
    {
        _img = GetComponent<Image>();
        _img.sprite = sprite;
        _img.color = Color.white;
    }

    public void SetDone()
    {
        _img.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    }
}
