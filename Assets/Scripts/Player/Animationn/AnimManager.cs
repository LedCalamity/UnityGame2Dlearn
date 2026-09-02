using UnityEngine;

public class AnimManager : MonoBehaviour
{
    PlayerControlMove move_ins;
    PlayerControlJump jump_ins;
    Animator animator;
    SpriteRenderer rd;
    void Start()
    {
        move_ins = GetComponent<PlayerControlMove>();
        jump_ins = GetComponent<PlayerControlJump>();
        animator = GetComponent<Animator>();
        rd = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        CheckMove();
        CheckDir();
    }
    void CheckMove()
    {
        if (move_ins.move_vec.magnitude >= 0.001f && jump_ins.is_grounded)
        {
            animator.SetInteger("AnimStatus", 1);
        }
        else animator.SetInteger("AnimStatus", 0);
    }
    void CheckDir()
    {
        if (move_ins.is_player_right) rd.flipX = true;
        else rd.flipX= false;
    }
}
