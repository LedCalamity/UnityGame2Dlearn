using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlJump : MonoBehaviour
{
    public float jump_mag = 5f;
    public bool is_grounded = true;
    int jump_time = 1, jump_ct = 0;
    Rigidbody2D rb;
    public LayerMask groundLayer;
    BoxCollider2D collider2d;
    InputManager input_manager;
    void OnEnable()
    {
        input_manager = GetComponent<InputManager>();
        input_manager.OnJump += Jump;
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<BoxCollider2D>();
        groundLayer = LayerMask.GetMask("Ground");
    }
    private void OnDisable()
    {
        input_manager.OnJump -= Jump;
    }
    void Jump()
    {
        if (jump_ct < jump_time)
        {
            handleJump();
            jump_ct++;
        }
    }
    void handleJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump_mag);
    }
    private void FixedUpdate()
    {
        is_grounded = Physics2D.CircleCast(transform.position, collider2d.size.x / 2, Vector2.down, 0.05f, groundLayer);
        if (is_grounded) jump_ct = 0;
    }
}
