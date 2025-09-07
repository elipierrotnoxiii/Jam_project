using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public bool canMove;

    [Header("References")]
    public Animator animator;
    //public Camera mainCamera;

    private Rigidbody rb;
    private Vector3 moveInput;
    private Vector3 moveVelocity;

    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        canMove = true;
    }

    void Update()
    {
        Move();
    }

    private void FixedUpdate() 
    {
        if (canMove == true)
        {
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        }
    }

    private void Move()
    {
        if (canMove == true)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");

            moveInput = new Vector3(moveX, 0f, moveZ).normalized;
            moveVelocity = moveInput * moveSpeed;

            if (moveX != 0 || moveZ != 0)
            {
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }
        }
    }
}
