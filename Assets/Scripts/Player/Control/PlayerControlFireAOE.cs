using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControlFireAOE : MonoBehaviour
{
    bool is_fire_aoe = false;
    float cur_time = 0f, cd = 5f;
    public Image skill_cd;
    InputManager input_manager;
    private void Awake()
    {
        input_manager = GetComponent<InputManager>();
        input_manager.OnFireAOE += Skill_FireAOE;
    }
    private void Start()
    {
        if (UIManager.Instance != null)
        {
            skill_cd = UIManager.Instance.aoeCdImageCover;
        }
    }
    private void OnDisable()
    {
        input_manager.OnFireAOE -= Skill_FireAOE;
    }
    void Skill_FireAOE()
    {
        if (!is_fire_aoe && PlayerMana.Instance.IsAbundant(6))
        {
            is_fire_aoe = true;
            SkillManager.Instance.CallSkillFireAOETask(transform.position + new Vector3(0f, 0.7f, 0));
        }
    }
    private void Update()
    {
        if (is_fire_aoe)
        {
            cur_time += Time.deltaTime;
            if (cur_time > cd)
            {
                cur_time = 0f;
                is_fire_aoe = false;
            }
        }
        if (skill_cd != null)
        {
            skill_cd.fillAmount = cur_time == 0 ? 0 : (cd - cur_time) / cd;
        }
    }
}
