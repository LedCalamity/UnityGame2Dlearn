using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlMove : MonoBehaviour
{
    public float speed = 0.5f;
    public Vector2 move_vec;
    PlayerActions input;
    public bool is_player_right = true;
    float pre_player_x, cur_player_x;
    Rigidbody2D rb;
    InputManager input_manager;
    void OnEnable()
    {
        input_manager = GetComponent<InputManager>();
        input_manager.OnMove += Move;
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnDisable()
    {
        input_manager.OnMove -= Move;
    }
    void Move(Vector2 vecc)
    {
        move_vec = vecc;
    }
    void Update()
    {
        HandleMove();
        PlayerCheckDir();
    }
    void HandleMove()
    {
        rb.linearVelocityX = move_vec.x * speed;
    }
    void PlayerCheckDir()
    {
        pre_player_x = cur_player_x;
        cur_player_x = transform.position.x;
        if (Mathf.Abs(cur_player_x - pre_player_x) > 0.001f)
        {
            is_player_right = (cur_player_x - pre_player_x) > 0; //right or not
        }
    }
}
