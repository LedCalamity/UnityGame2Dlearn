using TMPro;
using UnityEngine;

public class TriggerDashHint : MonoBehaviour
{
    public TMP_Text texxt;
    TextMessageShow textMes;
    void Start()
    {
        textMes = texxt.GetComponent<TextMessageShow>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textMes.EnterDashHint();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textMes.ExitDashHint();
        }
    }
}
