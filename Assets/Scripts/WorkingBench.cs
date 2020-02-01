using System.Collections.Generic;
using UnityEngine;

public class WorkingBench : Interactable
{
    [Header("Settings")]
    [SerializeField]
    Sprite[] _qteActions;
    [SerializeField]
    private int _qteLength = 4;
    [SerializeField]
    private float _timePerQTE = 1.0f;

    [Header("Components")]
    [SerializeField]
    private QTEIndicator _indicatorPrefab;
    [SerializeField]
    private Transform _indicatorContainer;
    private List<QTEIndicator> _indicators = new List<QTEIndicator>();

    private Character _interactingCharacter;
    private bool _isInQTE = false;
    private int[] _currentQTE;
    private int _currentInputIndex = 0;
    public bool InQTE => _isInQTE;


    public bool Interact(Item item, Character c)
    {
        if (!_IsValidItem(item))
            return false;

        _interactingCharacter = c;
        _interactingCharacter.disableMovement = true;
        _StartQTE();
        Destroy(item.gameObject);
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
        }
    }

    private bool _IsValidItem(Item itm)
    {
        return true; // TODO
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
