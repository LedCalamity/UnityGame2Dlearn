using UnityEngine;

public class UnlockGravityReverse : MonoBehaviour
{
    void Start()
    {
        if(SaveManager.Instance != null && SaveManager.Instance.GravityReverseUnlocked)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.collider.CompareTag("Player") || SaveManager.Instance == null)
        {
            return;
        }

        PlayerControlGravityReverse gravity_reverse = collision.collider.GetComponent<PlayerControlGravityReverse>();
        if(gravity_reverse == null)
        {
            return;
        }

        SaveManager.Instance.UnlockGravityReverse();
        gravity_reverse.SetUnlocked(true);
        Destroy(gameObject);
    }
}
