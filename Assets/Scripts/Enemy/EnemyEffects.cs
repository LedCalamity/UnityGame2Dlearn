using UnityEngine;

public class EnemyEffects : MonoBehaviour
{
    public void BloodEffect()
    {
        EffectManager.Instance.GenerateHitEffect(transform.position);
    }
}
