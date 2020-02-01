﻿using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class WorkingBench : Interactable
{
    [Header("Settings")]
    [SerializeField]
    Sprite[] _qteActions;
    [SerializeField]
    private int _qteLength = 4;
    [SerializeField] private SpriteRenderer[] bodyPartSprites;

    [Header("Components")]
    [SerializeField]
    private QTEIndicator _indicatorPrefab;
    [SerializeField]
    private Transform _indicatorContainer;
    private List<QTEIndicator> _indicators = new List<QTEIndicator>();
    private Body _currentBody;
    public ItemType[] MissingParts => _currentBody.GetMissingBodyParts();

    //QTE
    private Character _interactingCharacter;
    private bool _isInQTE = false;
    private ItemType _currentItemType = ItemType.Head;
    private int[] _currentQTE;
    private int _currentInputIndex = 0;
    public bool InQTE => _isInQTE;


    protected override void Awake()
    {
        base.Awake();
        _CreateNewBody();
    }

    public bool Interact(Item item, Character c)
    {
        if (!_IsValidItem(item))
            return false;


        _interactingCharacter = c;
        _currentItemType = item.Type;
        Destroy(item.gameObject);

        if(_currentBody.BodyIsComplete())
        {
            _interactingCharacter.disableMovement = true;
            _StartQTE();
        }
        else
        {
            _SucessfullyAddedBodyPart(_currentItemType);
        }


        return true;
    }

    private void Update()
    {
        if (!_isInQTE)
            return;

    }

    public void SubmitQTE(int action)
    {
        if (!_isInQTE || _currentInputIndex >= _currentQTE.Length)
            return;

        bool correct = action == _currentQTE[_currentInputIndex];
        if(correct)
        {
            _NextQTEAction();
        }
        else
        {
            _StartQTE(); //Restart
        }
    }

    private void _StartQTE()
    {
        _isInQTE = true;
        _currentQTE = _CreateQTE(_qteLength);
        _currentInputIndex = -1;
        _NextQTEAction();


        //Create missing indicators
        for (int i = _indicators.Count; i < _currentQTE.Length; i++)
        {
            _indicators.Add(Instantiate(_indicatorPrefab, _indicatorContainer));
        }

        //Initialize QTE Indicators
        for (int i = 0; i < _indicators.Count; i++)
        {
            if (i < _currentQTE.Length)
            {
                _indicators[i].gameObject.SetActive(true);
                _indicators[i].Initialze(_qteActions[_currentQTE[i]]);
            }
            else
            {
                //dont destory, we keep it as object pool for performance reasons
                _indicators[i].gameObject.SetActive(false);
            }
        }

        _indicatorContainer.gameObject.SetActive(true);
    }

    private void _NextQTEAction()
    {

        _currentInputIndex++;
        if (_currentInputIndex < _currentQTE.Length)
        {
            if(_currentInputIndex > 0)
                _indicators[_currentInputIndex - 1].SetDone();
        }
        else
        {
            _isInQTE = false;
            _indicatorContainer.gameObject.SetActive(false);
            _interactingCharacter.disableMovement = false;
            _SucessfullyAddedBodyPart(_currentItemType);
        }
    }

    private void _SucessfullyAddedBodyPart(ItemType type)
    {
        if (_currentBody.BodyIsComplete()) 
            _CreateNewBody();

        _UpdateUI();
    }

    private void _CreateNewBody()
    {
        _currentBody = new Body(6);
        _UpdateUI();
    }

    private void _UpdateUI()
    {
        System.Array values = System.Enum.GetValues(typeof(ItemType));
        ItemType[] missing = _currentBody.GetMissingBodyParts();
        for (int i = 0; i < values.Length; i++)
        {
            ItemType t = (ItemType)values.GetValue(i);
            bodyPartSprites[i].gameObject.SetActive(!missing.Contains(t));

        }
    }

    private bool _IsValidItem(Item itm)
    {
        return _currentBody.AddBodypart(itm.Type);
    }

    private int[] _CreateQTE(int length)
    {
        int[] actions = new int[length];
        for(int i = 0; i < length; i++)
        {
            actions[i] = Random.Range(0, _qteActions.Length);
        }
        return actions;
    }
}
