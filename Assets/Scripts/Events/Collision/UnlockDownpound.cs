using UnityEngine;

public class UnlockDownpound : MonoBehaviour
{
    TextMessageShow mes;

    private void Start()
    {
        if(UIManager.Instance != null)
        {
            mes = UIManager.Instance.MessageText;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.collider.CompareTag("Player"))
        {
            return;
        }

        PlayerControlGroundPound groundPound = collision.collider.GetComponent<PlayerControlGroundPound>();
        if(groundPound == null)
        {
            return;
        }

        groundPound.is_unlocked = true;
        mes?.UnlockDownpound();
        Destroy(gameObject);
    }
}
