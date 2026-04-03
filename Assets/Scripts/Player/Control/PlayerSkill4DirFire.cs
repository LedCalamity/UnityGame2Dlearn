using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSkill4DirFire : MonoBehaviour
{
    PlayerActions input;
    bool is_4dir_fire = false;
    float cur_time = 0f, cd = 3f;
    public Image skill_cd;
    InputManager input_manager;
    private void Awake()
    {
        input_manager = GetComponent<InputManager>();
        input_manager.On4DirFire += Skill_4Dir_Fire;
    }
    private void OnDisable()
    {
        input_manager.On4DirFire -= Skill_4Dir_Fire;
    }
    void Skill_4Dir_Fire()
    {
        if(!is_4dir_fire && PlayerMana.Instance.IsAbundant(4))
        {
            is_4dir_fire = true;
            SkillManager.Instance.CallSkill4DirFireTask(transform.position + new Vector3(0, 0.5f, 0));
        }
    }
    private void Update()
    {
        if(is_4dir_fire)
        {
            cur_time += Time.deltaTime;
            if(cur_time > cd)
            {
                cur_time = 0f;
                is_4dir_fire = false;
            }
        }
        skill_cd.fillAmount = (cur_time == 0 ? 0 : (cd - cur_time) / cd);
    }
}
