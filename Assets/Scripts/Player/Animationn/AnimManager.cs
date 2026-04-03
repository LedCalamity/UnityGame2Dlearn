using UnityEngine;

public class AnimManager : MonoBehaviour
{
    PlayerControlMove move_ins;
    Animator animator;
    SpriteRenderer rd;
    void Start()
    {
        move_ins = GetComponent<PlayerControlMove>();
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
        if (move_ins.move_vec.magnitude >= 0.001f)
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
