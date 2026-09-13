using UnityEngine;

public class MaxHPAdder : MonoBehaviour
{
    [SerializeField] string pickup_id;
    [SerializeField, Min(1)] int max_hp_add_amount = 4;

    void Start()
    {
        if(SaveManager.Instance != null && SaveManager.Instance.IsPermanentPickupCollected(pickup_id))
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.collider.CompareTag("Player") || Playerhp.Instance == null || SaveManager.Instance == null)
        {
            return;
        }

        if(!SaveManager.Instance.TryCollectMaxHPUpgrade(pickup_id, max_hp_add_amount))
        {
            return;
        }

        Playerhp.Instance.SetMaxHealth(SaveManager.Instance.MaxHP, true);
        if(UIManager.Instance != null)
        {
            UIManager.Instance.MessageText?.CollectMaxHP(max_hp_add_amount);
        }
        Destroy(gameObject);
    }
}
