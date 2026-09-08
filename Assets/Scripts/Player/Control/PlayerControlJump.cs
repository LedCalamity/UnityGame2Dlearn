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
    PlayerControlGravityReverse gravity_reverse;
    void OnEnable()
    {
        input_manager = GetComponent<InputManager>();
        input_manager.OnJump += Jump;
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<BoxCollider2D>();
        gravity_reverse = GetComponent<PlayerControlGravityReverse>();
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
        float jump_direction = gravity_reverse != null ? -gravity_reverse.GravityDirection.y : 1f;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump_mag * jump_direction);
    }
    private void FixedUpdate()
    {
        Vector2 gravity_direction = gravity_reverse != null ? gravity_reverse.GravityDirection : Vector2.down;
        Bounds bounds = collider2d.bounds;
        float surface_y = gravity_direction.y < 0f ? bounds.min.y : bounds.max.y;
        Vector2 origin = new Vector2(bounds.center.x, surface_y - gravity_direction.y * 0.02f);
        Vector2 check_size = new Vector2(bounds.size.x * 0.8f, 0.02f);
        is_grounded = Physics2D.BoxCast(origin, check_size, 0f, gravity_direction, 0.05f, groundLayer);
        if (is_grounded) jump_ct = 0;
    }
}
