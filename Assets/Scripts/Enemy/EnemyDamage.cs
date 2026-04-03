using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            AudioManager.Instance.AudioPlay(3, "Hit_sef", false);
            collision.collider.GetComponent<Playerhp>().PlayerTakeDamage(2);
        }
    }
}
