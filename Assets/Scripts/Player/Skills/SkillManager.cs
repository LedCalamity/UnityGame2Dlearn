using System.Collections;
using UnityEngine;

//check mana-> effect->audio->logic
public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;
    public GameObject fire_bulletPrefab;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    public void CallSkill4DirFireTask(Vector3 pos)
    {
        StartCoroutine(Skill4DirFireTask(pos));
    }
    IEnumerator Skill4DirFireTask(Vector3 pos)
    {
        if (!PlayerMana.Instance.DeductMana(4)) yield break;
        //Effect
        EffectManager.Instance.Generate4DirFireEffect(pos);
        //Audio
        AudioManager.Instance.AudioPlay(2, "4DirFire_sef", false);
        //action(logic)
        Instantiate(fire_bulletPrefab, pos, Quaternion.Euler(0, 0, 45));
        Instantiate(fire_bulletPrefab, pos, Quaternion.Euler(0, 0, 135));
        Instantiate(fire_bulletPrefab, pos, Quaternion.Euler(0, 0, 225));
        Instantiate(fire_bulletPrefab, pos, Quaternion.Euler(0, 0, 315));

        yield return new WaitForSeconds(0.5f);
    }
    public void CallSkillFireAOETask(Vector3 pos)
    {
        StartCoroutine(SkillFireAOETask(pos));
    }
    IEnumerator SkillFireAOETask(Vector3 pos)
    {
        if(!PlayerMana.Instance.DeductMana(7)) yield break;
        var fire_aoe_object = EffectManager.Instance.GenerateFireAOEEffect(pos);
        
        AudioManager.Instance.AudioPlay(2, "FireAOE_sef", false);

        Collider2D[] hitrange = Physics2D.OverlapCircleAll(pos, fire_aoe_object.transform.localScale.x * fire_aoe_object.GetComponent<CircleCollider2D>().radius);
        //below is the place for logical manip. of aoe skill
        for (int i = 0; i < 5; i++) //hit 5 times consecutively
        {
            foreach(var hit in hitrange)
            {
                if(hit.CompareTag("Enemies"))
                {
                    hit.GetComponent<EnemyHP>().DeductHealth(1);
                }
            }
            yield return new WaitForSeconds(0.2f); //with interval 0.2s
        }
        
    }
}
