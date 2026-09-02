using UnityEngine;

public class EnemyRangedData : EnemyData
{
    [Header("Ranged Attack")]
    [SerializeField] GameObject chaser_bullet_prefab;
    [SerializeField] Transform bullet_spawn_point;
    [SerializeField, Min(0.1f)] float fire_interval = 1.5f;
    [SerializeField, Min(0)] int bullet_damage = 1;

    public GameObject ChaserBulletPrefab => chaser_bullet_prefab;
    public Transform BulletSpawnPoint => bullet_spawn_point;
    public float FireInterval => fire_interval;
    public int BulletDamage => bullet_damage;
}
