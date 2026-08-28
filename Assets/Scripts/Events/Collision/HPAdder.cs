using UnityEngine;

public class HPAdder : MonoBehaviour
{
    [SerializeField, Min(1)] int hp_add_amount = 2;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.collider.CompareTag("Player") || Playerhp.Instance == null)
        {
            return;
        }

        Playerhp.Instance.RestoreHealth(hp_add_amount);
        Destroy(gameObject);
    }
}
