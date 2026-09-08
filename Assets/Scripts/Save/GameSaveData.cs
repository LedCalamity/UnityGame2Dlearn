[System.Serializable]
public class GameSaveData
{
    public int save_version = 4;
    public int highest_unlocked_level = 0;
    public bool ground_pound_unlocked = false;
    public bool gravity_reverse_unlocked = false;
    public int max_hp = 10;
    public int max_mana = 10;
    public int bullet_damage = 2;
    public int chaser_bullet_damage = 1;
    public System.Collections.Generic.List<string> collected_permanent_pickup_ids = new System.Collections.Generic.List<string>();
}
