using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private Character _character;
    [SerializeField]
    private int _playerIndex = 0;

    private void Update()
    {
        float horizontal = Input.GetAxis($"Horizontal_P{_playerIndex}");
        float vertical = Input.GetAxis($"Vertical_P{_playerIndex}");
        _character.Move(new Vector2(horizontal, vertical));

        bool interact = Input.GetButtonDown($"Interact_P{_playerIndex}");
        if(interact)
        {
            _character.Interact();
        }


        for (int qteIdx = 0; qteIdx < 4; qteIdx++)
        {
            bool qte = Input.GetButtonDown($"QTE{qteIdx}_P{_playerIndex}");
            if (qte)
            {
                _character.SubmitQTEAction(qteIdx);
            }
        }
    }
}
