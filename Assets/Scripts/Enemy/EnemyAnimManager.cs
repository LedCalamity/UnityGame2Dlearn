using UnityEngine;

public class EnemyAnimManager : MonoBehaviour
{
    SpriteRenderer rd;
    EnemyFSM enemy_state;
    Animator enemy_animator;
    float last_x, cur_x;
    public bool is_face_right;
    void Start()
    {
        rd = GetComponent<SpriteRenderer>();
        enemy_state = GetComponent<EnemyFSM>();
        enemy_animator = GetComponent<Animator>();
        last_x = transform.position.x;
        cur_x = last_x;
    }
    void Update()
    {
        EnemyCheckStatus();
        EnemyCheckDir();
    }
    void EnemyCheckStatus()
    {
        enemy_animator.SetInteger("EnemyStatus", enemy_state.IsMoving ? 1 : 0);
    }
    void EnemyCheckDir()
    {
        last_x = cur_x;
        cur_x = transform.position.x;
        if (Mathf.Abs(cur_x - last_x) > 0.001f) is_face_right = (cur_x - last_x) > 0;  //avoid facing right all day long
        ApplyFacingDirection();
    }

    public void SetFacingDirection(bool face_right)
    {
        is_face_right = face_right;
        ApplyFacingDirection();
    }

    public void PlayAttack()
    {
        enemy_animator.SetTrigger("Attack");
    }

    void ApplyFacingDirection()
    {
        if (is_face_right) //right
        {
            rd.flipX = false;
        }
        else rd.flipX = true;
    }
}
