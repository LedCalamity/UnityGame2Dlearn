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

        PlayerControlGroundPound ground_pound = collision.collider.GetComponent<PlayerControlGroundPound>();
        if(ground_pound == null)
        {
            return;
        }

        SaveManager.Instance.UnlockGroundPound();
        ground_pound.SetUnlocked(true);
        mes?.UnlockDownpound();
        Destroy(gameObject);
    }
}
