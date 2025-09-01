using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("References")]

    //public Camera mainCamera;

    private Rigidbody rb;
    private Vector3 moveInput;
    private Vector3 moveVelocity;

    private void Start() {

        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveInput = new Vector3(moveX, 0f, moveZ).normalized;
        moveVelocity = moveInput * moveSpeed;

        /*
        
            Aca podemos setear la animacion 

        */


     

    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }
}
