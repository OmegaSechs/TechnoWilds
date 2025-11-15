using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    private PlayerInputHandler inputHandler;
    protected override void Awake()
    {
        base.Awake();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        moveInput = inputHandler.GetMoveInput();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        Move();
    }
}