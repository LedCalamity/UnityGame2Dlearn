using UnityEngine;

public class GemCollection : MonoBehaviour
{
    [SerializeField] GameObject target_object;
    [SerializeField, Min(0f)] float camera_focus_duration = 1.5f;

    int total_gems;
    int collected_gems;

    void Awake()
    {
        // Only count gems owned by this group, not a nested collection group.
        foreach(CollectGem gem in GetComponentsInChildren<CollectGem>())
        {
            if(gem.GetComponentInParent<GemCollection>() == this)
            {
                total_gems++;
            }
        }
    }

    public void CollectGem()
    {
        if(collected_gems >= total_gems) return;

        collected_gems++;
        if(UIManager.Instance != null)
        {
            UIManager.Instance.MessageText?.CollectGem(collected_gems, total_gems);
        }

        if(collected_gems == total_gems)
        {
            DestroyTarget();
        }
    }

    void DestroyTarget()
    {
        if(target_object == null) return;

        Vector3 focus_position = target_object.transform.position;
        Collider2D target_collider = target_object.GetComponentInChildren<Collider2D>();
        if(target_collider != null)
        {
            focus_position = target_collider.bounds.center;
        }

        CameraFocusController camera_focus = FindAnyObjectByType<CameraFocusController>();
        if(camera_focus != null && camera_focus_duration > 0f)
        {
            camera_focus.FocusAt(focus_position, camera_focus_duration);
        }

        Destroy(target_object);
    }
}
