using UnityEngine;

public class EnemyFlyFSM : EnemyFSM
{
    enum ChasePhase { Shooting, ChargePrepare, Charging, Recovery }

    EnemyFlyData fly_data;
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

    protected override void Awake()
    {
        base.Awake();
        if(!enabled)
        {
            return;
        }

        fly_data = enemy_data as EnemyFlyData;
        if(fly_data != null)
        {
            return;
        }

        Debug.LogError("EnemyFlyFSM needs an EnemyFlyData component.", this);
        enabled = false;
    }

    protected override void Start()
    {
        base.Start();
        if(!enabled || rb == null)
        {
            return;
        }

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
        if(patrol_wait_timer < fly_data.PatrolWaitTime)
        {
            return;
        }

        patrol_wait_timer = 0f;
        is_waiting_at_patrol_point = false;
        Vector2[] patrol_point_offsets = fly_data.PatrolPointOffsets;
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
        if(player_distance > fly_data.SightDistance)
        {
            return false;
        }

        if(player_distance <= Mathf.Epsilon)
        {
            return true;
        }

        Vector2 forward = enemyAnimManager.is_face_right ? Vector2.right : Vector2.left;
        float minimum_dot = Mathf.Cos(fly_data.SightAngle * 0.5f * Mathf.Deg2Rad);
        if(Vector2.Dot(forward, to_player.normalized) < minimum_dot)
        {
            return false;
        }

        int sight_layers = fly_data.PlayerLayer.value | fly_data.SightBlockingLayers.value;
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
        if(current_fire_time < fly_data.FireInterval)
        {
            return;
        }

        current_fire_time = 0f;
        if(!FireChaserBullet())
        {
            return;
        }

        shots_fired++;
        if(shots_fired < fly_data.ShotsBeforeCharge)
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
        if(phase_time < fly_data.ChargePrepareTime)
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
        if(phase_time < fly_data.RecoveryTime)
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
        Vector2[] patrol_point_offsets = fly_data.PatrolPointOffsets;
        if(patrol_point_offsets == null || patrol_point_offsets.Length == 0 || is_waiting_at_patrol_point)
        {
            StopMovement();
            return;
        }

        Vector2 target_position = patrol_origin + patrol_point_offsets[patrol_index];
        Vector2 difference = target_position - rb.position;
        if(difference.sqrMagnitude <= fly_data.PatrolReachDistance * fly_data.PatrolReachDistance)
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

        Vector2 next_position = Vector2.MoveTowards(rb.position, target_position, fly_data.FlyPatrolSpeed * Time.fixedDeltaTime);
        rb.MovePosition(next_position);
    }

    void MoveDuringCharge()
    {
        phase_time += Time.fixedDeltaTime;
        if(phase_time >= fly_data.ChargeDuration)
        {
            BeginRecovery();
            return;
        }

        rb.MovePosition(rb.position + charge_direction * fly_data.ChargeSpeed * Time.fixedDeltaTime);
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
        if(difference.sqrMagnitude <= fly_data.PatrolReachDistance * fly_data.PatrolReachDistance)
        {
            SelectRandomShootingTarget();
            difference = shooting_move_target - rb.position;
        }

        float shooting_move_speed = fly_data.FlyPatrolSpeed * 0.5f;
        Vector2 next_position = Vector2.MoveTowards(
            rb.position,
            shooting_move_target,
            shooting_move_speed * Time.fixedDeltaTime);
        rb.MovePosition(next_position);
    }

    void SelectRandomShootingTarget()
    {
        Vector2[] patrol_point_offsets = fly_data.PatrolPointOffsets;
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
        GameObject chaser_bullet_prefab = fly_data.ChaserBulletPrefab;
        Transform bullet_spawn_point = fly_data.BulletSpawnPoint;
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
        bullet_controller.Init(player, fly_data.BulletDamage);
        return true;
    }

    void UpdateBulletSpawnDirection(bool face_right)
    {
        Transform bullet_spawn_point = fly_data.BulletSpawnPoint;
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
        return fly_data.SightOrigin != null ? fly_data.SightOrigin.position : transform.position;
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
        bool hit_stop_layer = (fly_data.ChargeStopLayers.value & (1 << collision.gameObject.layer)) != 0;
        if(hit_player || hit_stop_layer)
        {
            BeginRecovery();
        }
    }

    void OnDrawGizmosSelected()
    {
        EnemyFlyData draw_data = GetComponent<EnemyFlyData>();
        if(draw_data == null)
        {
            return;
        }

        Vector2 origin = draw_data.SightOrigin != null ? draw_data.SightOrigin.position : transform.position;
        EnemyAnimManager anim_manager = GetComponent<EnemyAnimManager>();
        Vector2 forward = anim_manager != null && anim_manager.is_face_right ? Vector2.right : Vector2.left;
        Vector2 left_edge = Quaternion.Euler(0f, 0f, draw_data.SightAngle * 0.5f) * forward;
        Vector2 right_edge = Quaternion.Euler(0f, 0f, -draw_data.SightAngle * 0.5f) * forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, draw_data.SightDistance);
        Gizmos.DrawLine(origin, origin + left_edge * draw_data.SightDistance);
        Gizmos.DrawLine(origin, origin + right_edge * draw_data.SightDistance);

        const int segment_count = 16;
        Vector2 previous_point = origin + right_edge * draw_data.SightDistance;
        for(int segment = 1; segment <= segment_count; segment++)
        {
            float angle = Mathf.Lerp(-draw_data.SightAngle * 0.5f, draw_data.SightAngle * 0.5f, segment / (float)segment_count);
            Vector2 direction = Quaternion.Euler(0f, 0f, angle) * forward;
            Vector2 current_point = origin + direction * draw_data.SightDistance;
            Gizmos.DrawLine(previous_point, current_point);
            previous_point = current_point;
        }
    }
}
