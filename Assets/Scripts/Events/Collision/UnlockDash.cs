using TMPro;
using UnityEngine;

public class UnlockDash : MonoBehaviour
{
    public TMP_Text texxt;
    TextMessageShow mes;
    private void Start()
    {
        mes = texxt.GetComponent<TextMessageShow>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            PlayerControlDash dash = collision.collider.GetComponent<PlayerControlDash>();
            dash.is_unlocked = true;
            Destroy(gameObject);
            mes.UnlockDash();
            Debug.Log("Dash unlocked!");
        }
    }
}
