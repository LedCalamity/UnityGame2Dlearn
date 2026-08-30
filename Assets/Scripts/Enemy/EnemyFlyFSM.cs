using UnityEngine;

public class EnemyFlyFSM : EnemyFSM
{
    enum ChasePhase { Shooting, ChargePrepare, Charging, Recovery }

    [Header("Patrol")]
    [SerializeField] Vector2[] patrol_point_offsets = { new Vector2(-3f, 0f), new Vector2(0f, 2f), new Vector2(3f, 0f) };
    [SerializeField, Min(0f)] float fly_patrol_speed = 2f;
    [SerializeField, Min(0.01f)] float patrol_reach_distance = 0.05f;
    [SerializeField, Min(0f)] float patrol_wait_time = 0.5f;

    [Header("Sight")]
    [SerializeField] Transform sight_origin;
    [SerializeField, Min(0f)] float sight_distance = 8f;
    [SerializeField, Range(0f, 360f)] float sight_angle = 90f;
    [SerializeField] LayerMask player_layer = 1 << 3;
    [SerializeField] LayerMask sight_blocking_layers = (1 << 6) | (1 << 7);

    [Header("Shooting")]
    [SerializeField] GameObject chaser_bullet_prefab;
    [SerializeField] Transform bullet_spawn_point;
    [SerializeField, Min(0.1f)] float fire_interval = 1f;
    [SerializeField, Min(0)] int bullet_damage = 1;
    [SerializeField, Min(1)] int shots_before_charge = 3;

    [Header("Charge")]
    [SerializeField, Min(0f)] float charge_prepare_time = 0.6f;
    [SerializeField, Min(0f)] float charge_speed = 8f;
    [SerializeField, Min(0.01f)] float charge_duration = 0.8f;
    [SerializeField, Min(0f)] float recovery_time = 0.8f;
    [SerializeField] LayerMask charge_stop_layers = (1 << 6) | (1 << 7);

    ChasePhase chase_phase;
    Vector2 patrol_origin;
    Vector2 charge_direction;
    Vector2 shooting_move_target;
    Collider2D player_collider;
    int patrol_index;
    int shots_fired;
    float patrol_wait_timer;
    float current_fire_time;
    float phase_time;
    bool is_waiting_at_patrol_point;
    bool has_shooting_move_target;

    public override bool IsMoving =>
        (state == States.Patrol && !is_waiting_at_patrol_point) ||
        (state == States.Chase &&
            (chase_phase == ChasePhase.Charging ||
            (chase_phase == ChasePhase.Shooting && has_shooting_move_target)));

    protected override void Start()
    {
        base.Start();
        patrol_origin = rb.position;
    }

    protected override void PrepareStateUpdate()
    {
        // Flight movement is applied through Rigidbody2D in FixedUpdate.
    }

    protected override void UpdatePatrolState(bool can_see_player)
    {
        if(can_see_player)
        {
            SwitchState(States.Chase);
            return;
        }

        if(!is_waiting_at_patrol_point)
        {
            return;
        }

        patrol_wait_timer += Time.deltaTime;
        if(patrol_wait_timer < patrol_wait_time)
        {
            return;
        }

        patrol_wait_timer = 0f;
        is_waiting_at_patrol_point = false;
        if(patrol_point_offsets != null && patrol_point_offsets.Length > 0)
        {
            patrol_index = (patrol_index + 1) % patrol_point_offsets.Length;
        }
    }

    protected override bool CheckEyeSight()
    {
        if(!TryFindPlayer())
        {
            return false;
        }

        Vector2 origin = GetSightOrigin();
        Vector2 to_player = GetPlayerCenter() - origin;
        float player_distance = to_player.magnitude;
        if(player_distance > sight_distance)
        {
            return false;
        }

        if(player_distance <= Mathf.Epsilon)
        {
            return true;
        }

        Vector2 forward = enemyAnimManager.is_face_right ? Vector2.right : Vector2.left;
        float minimum_dot = Mathf.Cos(sight_angle * 0.5f * Mathf.Deg2Rad);
        if(Vector2.Dot(forward, to_player.normalized) < minimum_dot)
        {
            return false;
        }

        int sight_layers = player_layer.value | sight_blocking_layers.value;
        RaycastHit2D hit = Physics2D.Raycast(origin, to_player.normalized, player_distance, sight_layers);
        bool can_see_player = IsPlayerCollider(hit.collider);
        Debug.DrawRay(origin, to_player, can_see_player ? Color.green : Color.red);
        if(can_see_player)
        {
            player_collider = hit.collider;
        }

        return can_see_player;
    }

    protected override void UpdateChaseState(bool can_see_player)
    {
        if(!TryFindPlayer())
        {
            SwitchState(States.Idle);
            return;
        }

        if(chase_phase != ChasePhase.Charging)
        {
            bool face_right = GetPlayerCenter().x >= rb.position.x;
            enemyAnimManager.SetFacingDirection(face_right);
            UpdateBulletSpawnDirection(face_right);
        }

        switch(chase_phase)
        {
            case ChasePhase.Shooting:
                UpdateShooting(can_see_player);
                break;
            case ChasePhase.ChargePrepare:
                UpdateChargePrepare();
                break;
            case ChasePhase.Charging:
                break;
            case ChasePhase.Recovery:
                UpdateRecovery(can_see_player);
                break;
        }
    }

    protected override void OnStateEntered(States new_state)
    {
        StopMovement();

        if(new_state == States.Chase)
        {
            ResetShootingPhase();
            return;
        }

        chase_phase = ChasePhase.Shooting;
        current_fire_time = 0f;
        phase_time = 0f;
        shots_fired = 0;
    }

    void FixedUpdate()
    {
        if(rb == null)
        {
            return;
        }

        if(state == States.Patrol)
        {
            MoveAlongPatrolPoints();
            return;
        }

        if(state == States.Chase && chase_phase == ChasePhase.Charging)
        {
            MoveDuringCharge();
            return;
        }

        if(state == States.Chase && chase_phase == ChasePhase.Shooting)
        {
            MoveDuringShooting();
            return;
        }

        StopMovement();
    }

    void UpdateShooting(bool can_see_player)
    {
        if(UpdateLostSight(can_see_player) || !can_see_player)
        {
            return;
        }

        current_fire_time += Time.deltaTime;
        if(current_fire_time < fire_interval)
        {
            return;
        }

        current_fire_time = 0f;
        if(!FireChaserBullet())
        {
            return;
        }

        shots_fired++;
        if(shots_fired < shots_before_charge)
        {
            return;
        }

        chase_phase = ChasePhase.ChargePrepare;
        phase_time = 0f;
        has_shooting_move_target = false;
    }

    void UpdateChargePrepare()
    {
        phase_time += Time.deltaTime;
        if(phase_time < charge_prepare_time)
        {
            return;
        }

        Vector2 to_player = GetPlayerCenter() - rb.position;
        if(to_player.sqrMagnitude <= Mathf.Epsilon)
        {
            to_player = enemyAnimManager.is_face_right ? Vector2.right : Vector2.left;
        }

        charge_direction = to_player.normalized;
        if(Mathf.Abs(charge_direction.x) > 0.01f)
        {
            enemyAnimManager.SetFacingDirection(charge_direction.x > 0f);
        }

        chase_phase = ChasePhase.Charging;
        phase_time = 0f;
    }

    void UpdateRecovery(bool can_see_player)
    {
        phase_time += Time.deltaTime;
        if(phase_time < recovery_time)
        {
            return;
        }

        if(can_see_player)
        {
            ResetShootingPhase();
        }
        else
        {
            SwitchState(States.Idle);
        }
    }

    void MoveAlongPatrolPoints()
    {
        if(patrol_point_offsets == null || patrol_point_offsets.Length == 0 || is_waiting_at_patrol_point)
        {
            StopMovement();
            return;
        }

        Vector2 target_position = patrol_origin + patrol_point_offsets[patrol_index];
        Vector2 difference = target_position - rb.position;
        if(difference.sqrMagnitude <= patrol_reach_distance * patrol_reach_distance)
        {
            rb.MovePosition(target_position);
            StopMovement();
            is_waiting_at_patrol_point = true;
            patrol_wait_timer = 0f;
            return;
        }

        if(Mathf.Abs(difference.x) > 0.01f)
        {
            enemyAnimManager.SetFacingDirection(difference.x > 0f);
        }

        Vector2 next_position = Vector2.MoveTowards(rb.position, target_position, fly_patrol_speed * Time.fixedDeltaTime);
        rb.MovePosition(next_position);
    }

    void MoveDuringCharge()
    {
        phase_time += Time.fixedDeltaTime;
        if(phase_time >= charge_duration)
        {
            BeginRecovery();
            return;
        }

        rb.MovePosition(rb.position + charge_direction * charge_speed * Time.fixedDeltaTime);
    }

    void MoveDuringShooting()
    {
        if(!has_shooting_move_target)
        {
            SelectRandomShootingTarget();
        }

        if(!has_shooting_move_target)
        {
            StopMovement();
            return;
        }

        Vector2 difference = shooting_move_target - rb.position;
        if(difference.sqrMagnitude <= patrol_reach_distance * patrol_reach_distance)
        {
            SelectRandomShootingTarget();
            difference = shooting_move_target - rb.position;
        }

        float shooting_move_speed = fly_patrol_speed * 0.5f;
        Vector2 next_position = Vector2.MoveTowards(
            rb.position,
            shooting_move_target,
            shooting_move_speed * Time.fixedDeltaTime);
        rb.MovePosition(next_position);
    }

    void SelectRandomShootingTarget()
    {
        if(patrol_point_offsets == null || patrol_point_offsets.Length == 0)
        {
            has_shooting_move_target = false;
            return;
        }

        float minimum_x = patrol_point_offsets[0].x;
        float maximum_x = minimum_x;
        float minimum_y = patrol_point_offsets[0].y;
        float maximum_y = minimum_y;

        for(int point_index = 1; point_index < patrol_point_offsets.Length; point_index++)
        {
            Vector2 point = patrol_point_offsets[point_index];
            minimum_x = Mathf.Min(minimum_x, point.x);
            maximum_x = Mathf.Max(maximum_x, point.x);
            minimum_y = Mathf.Min(minimum_y, point.y);
            maximum_y = Mathf.Max(maximum_y, point.y);
        }

        shooting_move_target = patrol_origin + new Vector2(
            Random.Range(minimum_x, maximum_x),
            Random.Range(minimum_y, maximum_y));
        has_shooting_move_target = true;
    }

    bool FireChaserBullet()
    {
        if(chaser_bullet_prefab == null || bullet_spawn_point == null)
        {
            Debug.LogWarning("EnemyFlyFSM needs a Chaser Bullet Prefab and Bullet Spawn Point.", this);
            return false;
        }

        GameObject bullet = Instantiate(chaser_bullet_prefab, bullet_spawn_point.position, Quaternion.identity);
        if(!bullet.TryGetComponent(out ChaserBulletController bullet_controller))
        {
            Debug.LogWarning("The enemy chaser bullet prefab needs ChaserBulletController.", bullet);
            Destroy(bullet);
            return false;
        }

        enemyAnimManager.PlayAttack();
        bullet_controller.Init(player, bullet_damage);
        return true;
    }

    void UpdateBulletSpawnDirection(bool face_right)
    {
        if(bullet_spawn_point == null)
        {
            return;
        }

        Vector3 local_position = bullet_spawn_point.localPosition;
        local_position.x = Mathf.Abs(local_position.x) * (face_right ? 1f : -1f);
        bullet_spawn_point.localPosition = local_position;
    }

    void ResetShootingPhase()
    {
        chase_phase = ChasePhase.Shooting;
        current_fire_time = 0f;
        phase_time = 0f;
        shots_fired = 0;
        SelectRandomShootingTarget();
        StopMovement();
    }

    void BeginRecovery()
    {
        chase_phase = ChasePhase.Recovery;
        phase_time = 0f;
        has_shooting_move_target = false;
        StopMovement();
    }

    void StopMovement()
    {
        if(rb != null && rb.linearVelocity.sqrMagnitude > 0f)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    Vector2 GetSightOrigin()
    {
        return sight_origin != null ? sight_origin.position : transform.position;
    }

    Vector2 GetPlayerCenter()
    {
        if(player_collider == null && player != null)
        {
            player_collider = player.GetComponent<Collider2D>();
            if(player_collider == null)
            {
                player_collider = player.GetComponentInChildren<Collider2D>();
            }
        }

        if(player_collider != null && player_collider.enabled)
        {
            return player_collider.bounds.center;
        }

        return player != null ? player.position : rb.position;
    }

    bool IsPlayerCollider(Collider2D target_collider)
    {
        if(target_collider == null)
        {
            return false;
        }

        Transform current_transform = target_collider.transform;
        while(current_transform != null)
        {
            if(current_transform.CompareTag("Player"))
            {
                return true;
            }

            current_transform = current_transform.parent;
        }

        return false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(chase_phase != ChasePhase.Charging)
        {
            return;
        }

        bool hit_player = collision.collider.CompareTag("Player");
        bool hit_stop_layer = (charge_stop_layers.value & (1 << collision.gameObject.layer)) != 0;
        if(hit_player || hit_stop_layer)
        {
            BeginRecovery();
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector2 origin = sight_origin != null ? sight_origin.position : transform.position;
        EnemyAnimManager anim_manager = GetComponent<EnemyAnimManager>();
        Vector2 forward = anim_manager != null && anim_manager.is_face_right ? Vector2.right : Vector2.left;
        Vector2 left_edge = Quaternion.Euler(0f, 0f, sight_angle * 0.5f) * forward;
        Vector2 right_edge = Quaternion.Euler(0f, 0f, -sight_angle * 0.5f) * forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, sight_distance);
        Gizmos.DrawLine(origin, origin + left_edge * sight_distance);
        Gizmos.DrawLine(origin, origin + right_edge * sight_distance);

        const int segment_count = 16;
        Vector2 previous_point = origin + right_edge * sight_distance;
        for(int segment = 1; segment <= segment_count; segment++)
        {
            float angle = Mathf.Lerp(-sight_angle * 0.5f, sight_angle * 0.5f, segment / (float)segment_count);
            Vector2 direction = Quaternion.Euler(0f, 0f, angle) * forward;
            Vector2 current_point = origin + direction * sight_distance;
            Gizmos.DrawLine(previous_point, current_point);
            previous_point = current_point;
        }
    }
}
