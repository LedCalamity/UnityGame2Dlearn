using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    EnemyData enemy_data;

    void Awake()
    {
        enemy_data = GetComponent<EnemyData>();
        if(enemy_data != null)
        {
            return;
        }

        Debug.LogError("EnemyDamage needs an EnemyData component.", this);
        enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            PlayerControlGroundPound groundPound = collision.collider.GetComponent<PlayerControlGroundPound>();
            if (groundPound != null && groundPound.IsInvincible) return;
            if(Playerhp.Instance.PlayerTakeDamage(enemy_data.ContactDamage))
            {
                AudioManager.Instance.AudioPlay(3, "Hit_sef", false);
            }
        }
    }
}
