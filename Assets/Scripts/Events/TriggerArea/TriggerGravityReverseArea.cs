using UnityEngine;

public class TriggerGravityReverseArea : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && other.TryGetComponent(out PlayerControlGravityReverse gravity_reverse))
        {
            gravity_reverse.EnterGravityReverseArea();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && other.TryGetComponent(out PlayerControlGravityReverse gravity_reverse))
        {
            gravity_reverse.ExitGravityReverseArea();
        }
    }
}
