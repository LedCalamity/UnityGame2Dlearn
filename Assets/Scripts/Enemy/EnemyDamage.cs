using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            PlayerControlGroundPound groundPound = collision.collider.GetComponent<PlayerControlGroundPound>();
            if (groundPound != null && groundPound.IsInvincible) return;
            AudioManager.Instance.AudioPlay(3, "Hit_sef", false);
            Playerhp.Instance.PlayerTakeDamage(2);
        }
    }
}
