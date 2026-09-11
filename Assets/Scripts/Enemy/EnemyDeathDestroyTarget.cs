using UnityEngine;

[RequireComponent(typeof(EnemyHP))]
public class EnemyDeathDestroyTarget : MonoBehaviour
{
    [SerializeField] GameObject target_object;
    [SerializeField, Min(0f)] float camera_focus_duration = 1.5f;

    EnemyHP enemy_hp;

    void Awake()
    {
        enemy_hp = GetComponent<EnemyHP>();
    }

    void OnEnable()
    {
        enemy_hp.Died += DestroyTarget;
    }

    void OnDisable()
    {
        enemy_hp.Died -= DestroyTarget;
    }

    void DestroyTarget()
    {
        if(target_object != null)
        {
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
}
