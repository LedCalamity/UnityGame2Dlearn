using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    [SerializeField, Min(0)] int damage = 0;

    void OnCollisionEnter2D(Collision2D collision)
    {
        DamagePlayer(collision.collider);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        DamagePlayer(collision.collider);
    }

    void DamagePlayer(Collider2D other)
    {
        if(!other.CompareTag("Player") || Playerhp.Instance == null)
        {
            return;
        }

        PlayerControlGroundPound ground_pound = other.GetComponent<PlayerControlGroundPound>();
        if(ground_pound != null && ground_pound.IsInvincible) return;

        // Playerhp controls the shared damage immunity interval.
        Playerhp.Instance.PlayerTakeDamage(damage);
    }
}
