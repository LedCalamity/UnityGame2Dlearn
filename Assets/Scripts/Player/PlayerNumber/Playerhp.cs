using UnityEngine;
using UnityEngine.UI;

public class Playerhp : MonoBehaviour
{
    public int max_player_hp = 10;
    int cur_player_hp = 0;
    public Image player_hp_bar;
    public static Playerhp Instance;
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
        if (cur_player_hp <= 0) GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDeath>().PlayerDie();
    }
    void UpdateBar()
    {
        cur_player_hp = Mathf.Clamp(cur_player_hp, 0, max_player_hp); //currently used to restrict the value of hp to not lower than 0
        player_hp_bar.fillAmount = (float)cur_player_hp / (float)max_player_hp;
    }
    public void PlayerTakeDamage(int dmg)
    {
        cur_player_hp -= dmg;
    }
    public void ResetHP()
    {
        cur_player_hp = max_player_hp;
    }
}
