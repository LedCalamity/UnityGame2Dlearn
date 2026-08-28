using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public enum States { Idle, Patrol, Chase }
    float patrol_speed = 1.5f, chase_speed = 3f;
    float eyesight = 10f;
    public States state;
    EnemyAnimManager enemyAnimManager;
    float idle_waiting_time = 2f, cur_waiting_time = 0f;
    float sight_remain_time = 2f, cur_lostsight_time = 0f;
    public float[] patrol_X = { -13, -5 };
    int patrol_ct = 0;
    Transform player;
    Rigidbody2D rb;
    void Start()
    {
        state = States.Idle;
        enemyAnimManager = GetComponent<EnemyAnimManager>(); 
        rb = GetComponent<Rigidbody2D>();
        TryFindPlayer();
    }
    void Update()
    {
        UpdateEnemyState();
    }
    void UpdateEnemyState()
    {
        if (Mathf.Abs(rb.linearVelocityX) > 0.01f) rb.linearVelocityX = 0f;
        bool can_see_player = CheckEyeSight();
        switch(state)
        {
            case States.Idle:
            {
                cur_waiting_time += Time.deltaTime;
                if (cur_waiting_time > idle_waiting_time)
                {
                    SwitchState(States.Patrol);
                }
                if(can_see_player) SwitchState(States.Chase);
                break;
            }
            case States.Patrol:
            {
                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(patrol_X[patrol_ct % 2], transform.position.y),
                    patrol_speed * Time.deltaTime); //patrolling between fixed points
                if (Mathf.Abs(transform.position.x - patrol_X[patrol_ct % 2]) <= 0.01f) patrol_ct++;
                if (can_see_player) SwitchState(States.Chase);
                break;
            }
            case States.Chase:
            {
                if (!TryFindPlayer())
                {
                    SwitchState(States.Idle);
                    break;
                }
                transform.position = Vector2.MoveTowards(transform.position, player.position, chase_speed * Time.deltaTime); //dash towards player
                if (!can_see_player)
                {
                    cur_lostsight_time += Time.deltaTime;
                }
                else
                {
                    cur_lostsight_time = 0f;
                }
                if(cur_lostsight_time > sight_remain_time)
                {
                    SwitchState(States.Idle);
                }
                break;
            }
        }
        
    }
    bool CheckEyeSight()
    {
        Vector2 dir = (enemyAnimManager.is_face_right ? Vector2.right : Vector2.left);
        Vector2 ori = (Vector2)transform.position + dir * (1f / 2f);
        RaycastHit2D hit = Physics2D.Raycast(ori, dir, eyesight, LayerMask.GetMask("PlayerLayer","Obstacle","Ground"));
        Debug.DrawRay(ori, dir * eyesight, Color.red);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            player = hit.collider.transform;
            return true;
        }
        return false;
    }
    bool TryFindPlayer()
    {
        if (player != null) return true;

        GameObject player_object = GameObject.FindGameObjectWithTag("Player");
        if (player_object == null) return false;

        player = player_object.transform;
        return true;
    }
    public void SwitchState(States l_state)
    {
        if (l_state == States.Chase && !TryFindPlayer()) l_state = States.Idle;
        state = l_state;
        if (l_state == States.Idle) cur_waiting_time = 0f;
        if (l_state == States.Chase) cur_lostsight_time = 0f;
    }
    public void SwitchStateStr(string l_state)
    {
        if (l_state == "Chase") SwitchState(States.Chase);
    }
}
