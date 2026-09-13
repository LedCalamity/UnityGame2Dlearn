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

        int previous_hp = Playerhp.Instance.CurrentHealth;
        Playerhp.Instance.RestoreHealth(hp_add_amount);
        if(UIManager.Instance != null)
        {
            UIManager.Instance.MessageText?.CollectHP(Playerhp.Instance.CurrentHealth - previous_hp);
        }
        Destroy(gameObject);
    }
}
