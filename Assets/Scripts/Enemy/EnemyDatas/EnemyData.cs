using UnityEngine;

[DisallowMultipleComponent]
public class EnemyData : MonoBehaviour
{
    [Header("Health And Damage")]
    [SerializeField, Min(1)] int max_hp = 10;
    [SerializeField, Min(0)] int contact_damage = 2;

    [Header("Movement")]
    [SerializeField, Min(0f)] float patrol_speed = 1.5f;
    [SerializeField, Min(0f)] float chase_speed = 3f;
    [SerializeField] float[] patrol_X = { -13f, -5f };

    [Header("State")]
    [SerializeField, Min(0f)] float eyesight = 10f;
    [SerializeField, Min(0f)] float idle_waiting_time = 2f;
    [SerializeField, Min(0f)] float sight_remain_time = 2f;

    public int MaxHp => max_hp;
    public int ContactDamage => contact_damage;
    public float PatrolSpeed => patrol_speed;
    public float ChaseSpeed => chase_speed;
    public float[] PatrolX => patrol_X;
    public float Eyesight => eyesight;
    public float IdleWaitingTime => idle_waiting_time;
    public float SightRemainTime => sight_remain_time;
}
