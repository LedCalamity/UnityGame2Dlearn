using UnityEngine;

public class EnemyFlyData : EnemyRangedData
{
    [Header("Flying Patrol")]
    [SerializeField] Vector2[] patrol_point_offsets =
    {
        new Vector2(-3f, -2f),
        new Vector2(-3f, 2f),
        new Vector2(3f, 2f),
        new Vector2(3f, -2f)
    };
    [SerializeField, Min(0f)] float fly_patrol_speed = 2f;
    [SerializeField, Min(0.01f)] float patrol_reach_distance = 0.05f;
    [SerializeField, Min(0f)] float patrol_wait_time = 0.5f;

    [Header("Flying Sight")]
    [SerializeField] Transform sight_origin;
    [SerializeField, Min(0f)] float sight_distance = 8f;
    [SerializeField, Range(0f, 360f)] float sight_angle = 90f;
    [SerializeField] LayerMask player_layer;
    [SerializeField] LayerMask sight_blocking_layers;

    [Header("Flying Attack")]
    [SerializeField, Min(1)] int shots_before_charge = 3;
    [SerializeField, Min(0f)] float charge_prepare_time = 0.6f;
    [SerializeField, Min(0f)] float charge_speed = 8f;
    [SerializeField, Min(0.01f)] float charge_duration = 0.8f;
    [SerializeField, Min(0f)] float recovery_time = 0.8f;
    [SerializeField] LayerMask charge_stop_layers;

    void Reset()
    {
        player_layer = LayerMask.GetMask("PlayerLayer");
        sight_blocking_layers = LayerMask.GetMask("Ground", "Obstacle");
        charge_stop_layers = LayerMask.GetMask("Ground", "Obstacle");
    }

    public Vector2[] PatrolPointOffsets => patrol_point_offsets;
    public float FlyPatrolSpeed => fly_patrol_speed;
    public float PatrolReachDistance => patrol_reach_distance;
    public float PatrolWaitTime => patrol_wait_time;
    public Transform SightOrigin => sight_origin;
    public float SightDistance => sight_distance;
    public float SightAngle => sight_angle;
    public LayerMask PlayerLayer => player_layer;
    public LayerMask SightBlockingLayers => sight_blocking_layers;
    public int ShotsBeforeCharge => shots_before_charge;
    public float ChargePrepareTime => charge_prepare_time;
    public float ChargeSpeed => charge_speed;
    public float ChargeDuration => charge_duration;
    public float RecoveryTime => recovery_time;
    public LayerMask ChargeStopLayers => charge_stop_layers;
}
