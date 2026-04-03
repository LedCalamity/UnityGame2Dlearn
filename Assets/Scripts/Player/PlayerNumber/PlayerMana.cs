using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    public int max_player_mana = 10;
    int cur_mana = 0;
    public Image mana_bar;
    float mana_add_interval = 2f, cur_mn_time = 0f;
    public static PlayerMana Instance;
    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    void Start()
    {
        cur_mana = max_player_mana;
    }
    void Update()
    {
        AutoManaInc();
        UpdateRender();
    }
    void UpdateRender()
    {
        mana_bar.fillAmount = (float)cur_mana / (float)max_player_mana;
    }
    void AutoManaInc()
    {
        cur_mn_time += Time.deltaTime;
        if(cur_mn_time > mana_add_interval)
        {
            cur_mn_time = 0;
            if (cur_mana != max_player_mana) DeductMana(-1);
        }
    }
    public bool DeductMana(int l_mana)
    {
        if(l_mana > cur_mana)
        {
            return false;
        }
        cur_mana -= l_mana;
        return true;
    }
    public bool IsAbundant(int l_mana)
    {
        if (l_mana > cur_mana)
        {
            return false;
        }
        return true;
    }
}
