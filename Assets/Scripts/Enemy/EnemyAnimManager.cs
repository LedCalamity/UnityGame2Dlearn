using UnityEngine;
using UnityEngine.SceneManagement;

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
    }
    void Update()
    {
        EnemyCheckStatus();
        EnemyCheckDir();
    }
    void EnemyCheckStatus()
    {
        if (enemy_state.state == EnemyFSM.States.Patrol || enemy_state.state == EnemyFSM.States.Chase) 
        {
            enemy_animator.SetInteger("EnemyStatus", 1);
        }
        else if (enemy_state.state == EnemyFSM.States.Idle)
        {
            enemy_animator.SetInteger("EnemyStatus", 0);
        }
    }
    void EnemyCheckDir()
    {
        last_x = cur_x;
        cur_x = transform.position.x;
        if (Mathf.Abs(cur_x - last_x) > 0.001f) is_face_right = (cur_x - last_x) > 0;  //avoid facing right all day long
        if (is_face_right) //right
        {
            rd.flipX = false;
        }
        else rd.flipX = true;
    }
}
