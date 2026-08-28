using UnityEngine;

public class TriggerDropDeath : MonoBehaviour
{
    GameObject death_player;
    void Start()
    {
        death_player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            death_player.GetComponent<PlayerDeath>().PlayerDie();
        }
        else if(collider.CompareTag("Enemies"))
        {
            EnemyDieDrop(collider);
        }
    }

    private void EnemyDieDrop(Collider2D enemy_collider)
    {
        Rigidbody2D enemy_rb = enemy_collider.attachedRigidbody;

        if(enemy_rb != null)
        {
            Destroy(enemy_rb.gameObject);
        }
        else
        {
            Destroy(enemy_collider.gameObject);
        }
    }
}
