using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField, Min(0)] int contact_damage = 2;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            PlayerControlGroundPound groundPound = collision.collider.GetComponent<PlayerControlGroundPound>();
            if (groundPound != null && groundPound.IsInvincible) return;
            if(Playerhp.Instance.PlayerTakeDamage(contact_damage))
            {
                AudioManager.Instance.AudioPlay(3, "Hit_sef", false);
            }
        }
    }
}
