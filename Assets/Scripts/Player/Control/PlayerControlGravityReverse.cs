using UnityEngine;

public class PlayerControlGravityReverse : MonoBehaviour
{
    [SerializeField, Min(0f)] float gravity_scale = 1f;
    [SerializeField] Transform center_anchor;

    Rigidbody2D rb;
    BoxCollider2D player_collider;
    SpriteRenderer sprite_renderer;
    InputManager input_manager;
    Vector2 normal_collider_offset;
    bool is_unlocked;
    bool can_reverse_here;

    public bool IsReversed { get; private set; }
    public Vector2 GravityDirection => IsReversed ? Vector2.up : Vector2.down;
    public Transform CenterAnchor => center_anchor != null ? center_anchor : transform;
    public Vector3 CenterPosition => player_collider.bounds.center;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        player_collider = GetComponent<BoxCollider2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        input_manager = GetComponent<InputManager>();
        normal_collider_offset = player_collider.offset;
        UpdateCenterAnchor();
        input_manager.OnGravityReverse += ToggleGravity;
        RestoreGravity();
    }

    void OnDisable()
    {
        if(input_manager != null)
        {
            input_manager.OnGravityReverse -= ToggleGravity;
        }
    }

    public void SetUnlocked(bool unlocked)
    {
        is_unlocked = unlocked;
        if(!is_unlocked) ResetGravityState();
    }

    public void EnterGravityReverseArea()
    {
        can_reverse_here = true;
    }

    public void ExitGravityReverseArea()
    {
        can_reverse_here = false;
    }

    public void ResetGravityState()
    {
        can_reverse_here = false;
        SetReversed(false);
    }

    public void RestoreGravity()
    {
        rb.gravityScale = IsReversed ? -gravity_scale : gravity_scale;
    }

    void ToggleGravity()
    {
        if(!is_unlocked || !can_reverse_here)
        {
            return;
        }

        SetReversed(!IsReversed);
    }

    void SetReversed(bool reversed)
    {
        IsReversed = reversed;

        Vector2 target_offset = normal_collider_offset;
        target_offset.y *= IsReversed ? -1f : 1f;
        Vector2 offset_difference = player_collider.offset - target_offset;
        Vector3 position_correction = transform.TransformVector(offset_difference);
        rb.position += new Vector2(position_correction.x, position_correction.y);
        player_collider.offset = target_offset;
        UpdateCenterAnchor();

        if(!Mathf.Approximately(rb.gravityScale, 0f))
        {
            RestoreGravity();
        }

        sprite_renderer.flipY = IsReversed;
    }

    void UpdateCenterAnchor()
    {
        if(center_anchor != null)
        {
            center_anchor.localPosition = player_collider.offset;
        }
    }
}
