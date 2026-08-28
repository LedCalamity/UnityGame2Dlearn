using UnityEngine;
using UnityEngine.UI;

public class Playerhp : MonoBehaviour
{
    public int max_player_hp = 10;
    int cur_player_hp = 0;
    public Image player_hp_bar;
    public static Playerhp Instance;

    public int CurrentHealth => cur_player_hp;
    public int MaxHealth => max_player_hp;

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
        CheckPlayerDeath();
        UpdateBar();
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
    public void PlayerTakeDamage(int dmg)
    {
        if(dmg <= 0)
        {
            return;
        }

        cur_player_hp = Mathf.Max(0, cur_player_hp - dmg);
        UpdateBar();
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
