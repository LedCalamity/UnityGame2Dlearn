using UnityEngine;

public class TriggerGemHint : MonoBehaviour
{
    TextMessageShow textMes;
    void Start()
    {
        if (UIManager.Instance != null)
        {
            textMes = UIManager.Instance.MessageText;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && textMes != null)
        {
            textMes.EnterGemHint();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && textMes != null)
        {
            textMes.ExitGemHint();
            Destroy(gameObject);
        }
    }
}
