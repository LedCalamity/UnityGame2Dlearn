using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int max_hp = 10;
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
        float pct = (float)cur_hp / (float)max_hp;
        hp_bar.transform.localScale = new Vector3(pct, hp_bar.transform.localScale.y, 1);
    }
    public void DeductHealth(int hp, bool generateBloodEffect = true)
    {
        cur_hp -= hp;

        if (generateBloodEffect)
        {
            GetComponent<EnemyEffects>().BloodEffect();
        }

        GetComponent<EnemyFSM>().SwitchStateStr("Chase");
    }
}
