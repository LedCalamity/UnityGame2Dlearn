using UnityEngine;

public class LifeAdder : MonoBehaviour
{
    [SerializeField, Min(1)] int life_add_amount = 1;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.collider.CompareTag("Player"))
        {
            return;
        }

        PlayerDeath player_death = collision.collider.GetComponent<PlayerDeath>();
        if(player_death == null)
        {
            return;
        }

        player_death.AddLife(life_add_amount);
        Destroy(gameObject);
    }
}
