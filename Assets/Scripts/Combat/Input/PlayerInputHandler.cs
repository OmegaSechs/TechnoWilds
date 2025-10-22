using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 GetMoveInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    public bool AttackPressed()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}
