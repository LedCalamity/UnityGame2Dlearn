using UnityEngine;

public class CollectGem : MonoBehaviour
{
    GemCollection collection;
    bool is_collected;

    void Awake()
    {
        collection = GetComponentInParent<GemCollection>();
        if(collection == null)
        {
            Debug.LogError("CollectGem needs a GemCollection parent.", this);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(is_collected || collection == null || !collision.collider.CompareTag("Player"))
        {
            return;
        }

        is_collected = true;
        collection.CollectGem();
        Destroy(gameObject);
    }
}
