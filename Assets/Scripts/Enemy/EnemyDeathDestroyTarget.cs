using UnityEngine;

[RequireComponent(typeof(EnemyHP))]
public class EnemyDeathDestroyTarget : MonoBehaviour
{
    [SerializeField] GameObject target_object;

    EnemyHP enemy_hp;

    void Awake()
    {
        enemy_hp = GetComponent<EnemyHP>();
    }

    void OnEnable()
    {
        enemy_hp.Died += DestroyTarget;
    }

    void OnDisable()
    {
        enemy_hp.Died -= DestroyTarget;
    }

    void DestroyTarget()
    {
        if(target_object != null)
        {
            Destroy(target_object);
        }
    }
}
