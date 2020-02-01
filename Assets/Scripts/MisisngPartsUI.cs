using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MisisngPartsUI : MonoBehaviour
{
    [SerializeField]
    private WorkingBench _targetWorkbench;
    [SerializeField]
    private Transform _slotContainer;
    [SerializeField]
    private Item _itemReference;
    private List<Image> _imageComponents= new List<Image>();

    private void OnEnable()
    {
        _targetWorkbench.OnBodyPartsChanged += _UpdateParts;
    }

    private void OnDisable()
    {
        _targetWorkbench.OnBodyPartsChanged -= _UpdateParts;
    }

    void _UpdateParts()
    {
        ItemType[] missingParts = _targetWorkbench.MissingParts;
        
        for (int i = _imageComponents.Count; i < missingParts.Length; i++)
            _CreateNewSlot();

        for(int i = 0; i < _imageComponents.Count; i++)
        {
            if(i < missingParts.Length)
            {
                _imageComponents[i].gameObject.SetActive(true);
                _imageComponents[i].sprite = _itemReference.GetSprite(missingParts[i]);
            }
            else
            {
                _imageComponents[i].gameObject.SetActive(false);
            }
        }
    }

    void _CreateNewSlot()
    {
        GameObject go = new GameObject("Slot");
        go.transform.SetParent(_slotContainer);
        _imageComponents.Add(go.AddComponent<Image>());
        go.transform.localScale = Vector3.one;
    }

}
