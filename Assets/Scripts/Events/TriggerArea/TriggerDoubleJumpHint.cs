using UnityEngine;

public class TriggerDoubleJumpHint : MonoBehaviour
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
            textMes.EnterDoubleJumpHint();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && textMes != null)
        {
            textMes.ExitDoubleJumpHint();
        }
    }
}
