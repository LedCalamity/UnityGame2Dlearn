using UnityEngine;
using UnityEngine.UI;

public class Playerhp : MonoBehaviour
{
    public int max_player_hp = 10;
    [SerializeField, Min(0f)] float damage_invincible_duration = 0.5f;

    int cur_player_hp = 0;
    float damage_invincible_remaining;
    public Image player_hp_bar;
    public static Playerhp Instance;

    public int CurrentHealth => cur_player_hp;
    public int MaxHealth => max_player_hp;
    public bool IsDamageInvincible => damage_invincible_remaining > 0f;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    void Start()
    {
        ResetHP();
    }
    void Update()
    {
        UpdateDamageInvincibility();
        CheckPlayerDeath();
        UpdateBar();
    }

    void UpdateDamageInvincibility()
    {
        if(damage_invincible_remaining <= 0f)
        {
            return;
        }

        damage_invincible_remaining = Mathf.Max(0f, damage_invincible_remaining - Time.deltaTime);
    }
    void CheckPlayerDeath()
    {
        if(cur_player_hp > 0)
        {
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null && player.TryGetComponent(out PlayerDeath player_death))
        {
            player_death.PlayerDie();
        }
    }
    void UpdateBar()
    {
        cur_player_hp = Mathf.Clamp(cur_player_hp, 0, max_player_hp); //currently used to restrict the value of hp to not lower than 0
        if(player_hp_bar != null)
        {
            player_hp_bar.fillAmount = max_player_hp > 0 ? (float)cur_player_hp / max_player_hp : 0;
        }
    }
    public bool PlayerTakeDamage(int dmg)
    {
        if(dmg <= 0 || cur_player_hp <= 0 || IsDamageInvincible)
        {
            return false;
        }

        cur_player_hp = Mathf.Max(0, cur_player_hp - dmg);
        damage_invincible_remaining = damage_invincible_duration;
        UpdateBar();
        return true;
    }

    public bool RestoreHealth(int amount)
    {
        if(amount <= 0 || cur_player_hp >= max_player_hp)
        {
            return false;
        }

        cur_player_hp = Mathf.Min(cur_player_hp + amount, max_player_hp);
        UpdateBar();
        return true;
    }
    public void ResetHP()
    {
        cur_player_hp = max_player_hp;
        UpdateBar();
    }
}
