using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlDash : MonoBehaviour
{
    Rigidbody2D rb;
    public float dash_mag = 1f;
    float dash_interval = 0.2f, cur_time = 0f, dash_break = 0.5f, cur_break = 0f;
    bool is_dashing = false, is_break = false;
    public bool is_unlocked = false;
    PlayerControlMove move_input;
    Vector2 dash_end_pos, dash_start_pos;
    InputManager input_manager;
    private void OnEnable()
    {
        input_manager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody2D>();
        move_input = GetComponent<PlayerControlMove>();
        input_manager.OnDash += Dash;
    }
    private void OnDisable()
    {
        input_manager.OnDash -= Dash;
    }
    void Dash()
    {
        if(!is_dashing && !is_break && is_unlocked) //not dashing or breaking or locked can dash 
        {
            StartDash();
        }
    }
    void StartDash()
    {
        dash_start_pos = rb.position;
        dash_end_pos = dash_start_pos + dash_mag * (Mathf.Abs(move_input.move_vec.x) > 0.0001f ? move_input.move_vec : new Vector2(1f, 0f));
        is_dashing = true;
        cur_time = 0f;
        rb.gravityScale = 0f;
        if (move_input != null) move_input.enabled = false;
    }
    private void FixedUpdate()
    {
        if(is_dashing)
        {
            cur_time += Time.fixedDeltaTime;
            if(cur_time > dash_interval)
            {
                rb.MovePosition(dash_end_pos);
                EndDash();
                return;
            }
            Vector2 pos = Vector2.Lerp(dash_start_pos, dash_end_pos, cur_time / dash_interval);
            rb.MovePosition(pos);
        }
        else if(is_break)
        {
            cur_break += Time.fixedDeltaTime;
            if (cur_break > dash_break) is_break = false;
        }
    }
    void EndDash()
    {
        is_dashing = false;
        is_break = true;
        cur_break = 0f;
        rb.gravityScale = 1f;
        if(move_input!=null) move_input.enabled = true;
    }
}
