using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float moveSpeed = 5f;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Vector2 moveInput;

    private float lastMoveX = 0f;
    private float lastMoveY = -1f;

    public virtual void UpdateAnimator(){

        if (moveInput.x != 0 || moveInput.y != 0){
            animator.SetFloat("Horizontal", moveInput.x);
            animator.SetFloat("Vertical", moveInput.y);
            lastMoveX = moveInput.x;
            lastMoveY = moveInput.y;
        }
        else
        {
            animator.SetFloat("Horizontal", lastMoveX);
            animator.SetFloat("Vertical", lastMoveY);
        }
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Move()
    {
        if (Input.GetKey(KeyCode.LeftShift)){
            moveSpeed =8f;
        }
        else
        {
            moveSpeed = 5f;
        };
        rb.linearVelocity = moveInput * moveSpeed;
        if (animator)
            animator.SetFloat("Speed", rb.linearVelocity.magnitude);
    }
}
