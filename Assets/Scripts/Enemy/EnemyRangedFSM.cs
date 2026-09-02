using UnityEngine;

public class EnemyRangedFSM : EnemyFSM
{
    EnemyRangedData ranged_data;
    float current_fire_time;

    public override bool IsMoving => state == States.Patrol;

    protected override void Awake()
    {
        base.Awake();
        if(!enabled)
        {
            return;
        }

        ranged_data = enemy_data as EnemyRangedData;
        if(ranged_data != null)
        {
            return;
        }

        Debug.LogError("EnemyRangedFSM needs an EnemyRangedData component.", this);
        enabled = false;
    }

    protected override void UpdateChaseState(bool can_see_player)
    {
        if(!TryFindPlayer())
        {
            SwitchState(States.Idle);
            return;
        }

        bool face_right = player.position.x >= transform.position.x;
        enemyAnimManager.SetFacingDirection(face_right);
        UpdateBulletSpawnDirection(face_right);

        if(UpdateLostSight(can_see_player) || !can_see_player)
        {
            return;
        }

        current_fire_time += Time.deltaTime;
        if(current_fire_time < ranged_data.FireInterval)
        {
            return;
        }

        current_fire_time = 0f;
        FireChaserBullet();
    }

    protected override void OnStateEntered(States new_state)
    {
        if(new_state == States.Chase)
        {
            current_fire_time = 0f;
        }
    }

    void UpdateBulletSpawnDirection(bool face_right)
    {
        Transform bullet_spawn_point = ranged_data.BulletSpawnPoint;
        if(bullet_spawn_point == null)
        {
            return;
        }

        Vector3 local_position = bullet_spawn_point.localPosition;
        local_position.x = Mathf.Abs(local_position.x) * (face_right ? 1f : -1f);
        bullet_spawn_point.localPosition = local_position;
    }

    void FireChaserBullet()
    {
        GameObject chaser_bullet_prefab = ranged_data.ChaserBulletPrefab;
        Transform bullet_spawn_point = ranged_data.BulletSpawnPoint;
        if(chaser_bullet_prefab == null || bullet_spawn_point == null)
        {
            Debug.LogWarning("EnemyRangedFSM needs a Chaser Bullet Prefab and Bullet Spawn Point.", this);
            return;
        }

        GameObject bullet = Instantiate(chaser_bullet_prefab, bullet_spawn_point.position, Quaternion.identity);
        if(!bullet.TryGetComponent(out ChaserBulletController bullet_controller))
        {
            Debug.LogWarning("The enemy chaser bullet prefab needs ChaserBulletController.", bullet);
            Destroy(bullet);
            return;
        }

        enemyAnimManager.PlayAttack();
        bullet_controller.Init(player, ranged_data.BulletDamage);
    }
}
