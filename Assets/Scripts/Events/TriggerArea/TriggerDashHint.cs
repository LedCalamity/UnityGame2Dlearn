using UnityEngine;

public class TriggerDashHint : MonoBehaviour
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
            textMes.EnterDashHint();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && textMes != null)
        {
            textMes.ExitDashHint();
        }
    }
}
