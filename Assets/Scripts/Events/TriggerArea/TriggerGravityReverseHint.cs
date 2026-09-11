using Unity.VisualScripting;
using UnityEngine;

public class TiggerGravityReverseHint : MonoBehaviour
{
    TextMessageShow text_mes;
    void Start()
    {
        if(UIManager.Instance!=null)
        {
            text_mes=UIManager.Instance.MessageText;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && text_mes != null)
        {
            text_mes.EnterGravityReverseHint();
        }
    }
    void OriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && text_mes != null)
        {
            text_mes.ExitGravityReverseHint();
        }
    }
}
