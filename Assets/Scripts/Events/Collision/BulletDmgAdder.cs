using UnityEngine;

public class BulletDmgAdder : MonoBehaviour
{
    [SerializeField] string pickup_id;
    [SerializeField, Min(1)] int bullet_damage_add_amount = 1;

    void Start()
    {
        if(SaveManager.Instance != null && SaveManager.Instance.IsPermanentPickupCollected(pickup_id))
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.collider.CompareTag("Player") || SaveManager.Instance == null || BulletData.Instance == null)
        {
            return;
        }

        if(!SaveManager.Instance.TryCollectBulletDamageUpgrade(pickup_id, bullet_damage_add_amount))
        {
            return;
        }

        BulletData.Instance.SetBulletDamage("Bullet", SaveManager.Instance.BulletDamage);
        if(UIManager.Instance != null)
        {
            UIManager.Instance.MessageText?.CollectBulletDamage(bullet_damage_add_amount);
        }
        Destroy(gameObject);
    }
}
