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
    private SpriteRenderer _qteIndicator;

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
            Debug.Log(".");
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
        _qteIndicator.gameObject.SetActive(true);
        _currentQTE = _CreateQTE(_qteLength);
        _currentInputIndex = -1;
        _NextQTEAction();
    }

    private void _NextQTEAction()
    {
        _currentInputIndex++;
        if(_currentInputIndex < _currentQTE.Length)
        {
            _qteIndicator.sprite = _qteActions[_currentQTE[_currentInputIndex]];
        }
        else
        {
            _isInQTE = false;
            _qteIndicator.gameObject.SetActive(false);
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
