using UnityEngine;

[RequireComponent(typeof(EnemyHP))]
public class EnemyDeathDamageTarget : MonoBehaviour
{
    [SerializeField] EnemyHP target_enemy;
    [SerializeField, Range(0f, 1f)] float damage_ratio = 0f;

    EnemyHP enemy_hp;

    void Awake()
    {
        enemy_hp = GetComponent<EnemyHP>();
    }

    void OnEnable()
    {
        enemy_hp.Died += DamageTarget;
    }

    void OnDisable()
    {
        enemy_hp.Died -= DamageTarget;
    }

    void DamageTarget()
    {
        if(target_enemy == null || target_enemy == enemy_hp || damage_ratio <= 0f)
        {
            return;
        }

        if(target_enemy.TryGetComponent(out EnemyData target_data))
        {
            // Use the target's maximum HP, not its remaining HP.
            int damage = Mathf.CeilToInt(target_data.MaxHp * damage_ratio);
            target_enemy.DeductHealth(damage);
        }
    }
}
