using UnityEngine;

public class ChaserBulletController : MonoBehaviour
{
    [SerializeField] float fire_speed = 8f;
    [SerializeField] float turn_speed = 900f;
    [SerializeField] float exist_time = 3f;
    [SerializeField] float chase_time = 2f;
    [SerializeField] float chase_range = 3f;
    [SerializeField, Min(0.02f)] float target_search_interval = 0.1f;

    float cur_t;
    float current_search_time;
    int damage;
    bool is_enemy_bullet;
    Transform targetTransform;
    Collider2D targetCollider;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(bool l_dir)
    {
        rb.rotation = l_dir ? 0f : 180f;
        InitPlayer();
    }

    public void InitPlayer()
    {
        is_enemy_bullet = false;
        damage = BulletData.Instance.getBulletData("ChaserBullet").damage;
        TryFindEnemyTarget();
        current_search_time = target_search_interval;
    }

    public void Init(Transform target, int bullet_damage)
    {
        is_enemy_bullet = true;
        SetTarget(target);
        damage = Mathf.Max(0, bullet_damage);

        if(targetTransform != null)
        {
            Vector2 direction = GetTargetPosition() - rb.position;
            rb.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
    }

    void Update()
    {
        cur_t += Time.deltaTime;
        if (cur_t >= exist_time) Destroy(gameObject);
    }

    void FixedUpdate()
    {
        float move_angle = rb.rotation;

        if(cur_t < chase_time && !is_enemy_bullet && targetTransform == null)
        {
            current_search_time -= Time.fixedDeltaTime;
            if(current_search_time <= 0f)
            {
                TryFindEnemyTarget();
                current_search_time = target_search_interval;
            }
        }

        if(cur_t < chase_time && targetTransform != null)
        {
            Vector2 difference = GetTargetPosition() - rb.position;
            float target_angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            move_angle = Mathf.MoveTowardsAngle(rb.rotation, target_angle, turn_speed * Time.fixedDeltaTime);
            rb.MoveRotation(move_angle);
        }

        float move_radians = move_angle * Mathf.Deg2Rad;
        rb.linearVelocity = new Vector2(Mathf.Cos(move_radians), Mathf.Sin(move_radians)) * fire_speed;
    }

    void TryFindEnemyTarget()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, chase_range, LayerMask.GetMask("EnemyLayer"));
        float closest_distance = float.PositiveInfinity;
        SetTarget(null);

        foreach(Collider2D target in targets)
        {
            EnemyHP enemy_hp = target.GetComponentInParent<EnemyHP>();
            if(enemy_hp == null)
            {
                continue;
            }

            Collider2D enemy_collider = enemy_hp.GetComponent<Collider2D>();
            if(enemy_collider == null)
            {
                enemy_collider = target;
            }

            float distance = (enemy_collider.bounds.center - (Vector3)rb.position).sqrMagnitude;
            if(distance >= closest_distance)
            {
                continue;
            }

            closest_distance = distance;
            SetTarget(enemy_hp.transform, enemy_collider);
        }
    }

    void SetTarget(Transform target, Collider2D known_collider = null)
    {
        targetTransform = target;
        targetCollider = known_collider;

        if(targetTransform == null || targetCollider != null)
        {
            return;
        }

        targetCollider = targetTransform.GetComponent<Collider2D>();
        if(targetCollider == null)
        {
            targetCollider = targetTransform.GetComponentInChildren<Collider2D>();
        }
    }

    Vector2 GetTargetPosition()
    {
        if(targetCollider != null && targetCollider.enabled)
        {
            return targetCollider.bounds.center;
        }

        return targetTransform != null ? targetTransform.position : rb.position;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Detectors"))
        {
            return;
        }

        if(is_enemy_bullet)
        {
            if(collider.CompareTag("Enemies"))
            {
                return;
            }

            if(collider.CompareTag("Player"))
            {
                PlayerControlGroundPound ground_pound = collider.GetComponent<PlayerControlGroundPound>();
                if(ground_pound == null || !ground_pound.IsInvincible)
                {
                    if(Playerhp.Instance.PlayerTakeDamage(damage))
                    {
                        AudioManager.Instance.AudioPlay(3, "Hit_sef", false);
                    }
                }
            }

            Destroy(gameObject);
            return;
        }

        if(collider.CompareTag("Player"))
        {
            return;
        }

        if(collider.CompareTag("Enemies") && collider.TryGetComponent(out EnemyHP enemy_hp))
        {
            enemy_hp.DeductHealth(damage);
        }

        Destroy(gameObject);
    }
}
