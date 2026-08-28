using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [Min(1)] public int max_hp = 10;
    int cur_hp = 0;
    public GameObject hp_bar;
    void Start()
    {
        cur_hp = max_hp;
    }

    // Update is called once per frame
    void Update()
    {
        CheckDeath();
        UpdateRender();
    }
    void CheckDeath()
    {
        if (cur_hp <= 0)
        {
            //EffectManager.Instance.GenerateDeathSlowMotion(0.3f);
            AudioManager.Instance.AudioPlay(1, "Death_sef", false);
            Destroy(gameObject);
        }
    }
    void UpdateRender()
    {
        if (hp_bar == null) return;
        float pct = max_hp > 0 ? Mathf.Clamp01((float)cur_hp / max_hp) : 0f;
        hp_bar.transform.localScale = new Vector3(pct, hp_bar.transform.localScale.y, 1);
    }
    public void DeductHealth(int hp, bool generateBloodEffect = true)
    {
        if(hp <= 0)
        {
            return;
        }

        cur_hp -= hp;

        if(generateBloodEffect && TryGetComponent(out EnemyEffects enemy_effects))
        {
            enemy_effects.BloodEffect();
        }

        if(TryGetComponent(out EnemyFSM enemy_fsm))
        {
            enemy_fsm.OnDamaged();
        }
    }
}
