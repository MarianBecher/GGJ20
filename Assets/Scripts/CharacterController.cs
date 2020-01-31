using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private Character _character;
    [SerializeField]
    private int _playerIndex = 0;

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal_" + _playerIndex);
        float vertical = Input.GetAxis("Vertical_" + _playerIndex);
        _character.Move(new Vector2(horizontal, vertical));
    }
}
