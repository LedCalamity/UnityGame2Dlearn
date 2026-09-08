using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour  //when permanant change occurs, here to record
{
    public static SaveManager Instance;

    [SerializeField] string save_file_name = "game_save.json";

    GameSaveData save_data;

    string SavePath => Path.Combine(Application.persistentDataPath, save_file_name);

    public int HighestUnlockedLevel => save_data.highest_unlocked_level;
    public bool GroundPoundUnlocked => save_data.ground_pound_unlocked;
    public bool GravityReverseUnlocked => save_data.gravity_reverse_unlocked;
    public int MaxHP => save_data.max_hp;
    public int BulletDamage => save_data.bullet_damage;

    void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }
        else Destroy(gameObject);
    }

    public bool IsLevelUnlocked(int level_index)
    {
        return level_index <= save_data.highest_unlocked_level;
    }

    public void UnlockLevel(int level_index)
    {
        if(level_index <= save_data.highest_unlocked_level)
        {
            return;
        }

        save_data.highest_unlocked_level = level_index;
        Save();
    }

    public void UnlockGroundPound()
    {
        if(save_data.ground_pound_unlocked)
        {
            return;
        }

        save_data.ground_pound_unlocked = true;
        Save();
    }

    public void UnlockGravityReverse()
    {
        if(save_data.gravity_reverse_unlocked)
        {
            return;
        }

        save_data.gravity_reverse_unlocked = true;
        Save();
    }

    public bool IsPermanentPickupCollected(string pickup_id)
    {
        return save_data.collected_permanent_pickup_ids.Contains(pickup_id);
    }

    public bool TryCollectMaxHPUpgrade(string pickup_id, int amount)
    {
        if(string.IsNullOrWhiteSpace(pickup_id) || amount <= 0 || IsPermanentPickupCollected(pickup_id))
        {
            return false;
        }

        save_data.collected_permanent_pickup_ids.Add(pickup_id);
        save_data.max_hp += amount;
        Save();
        return true;
    }

    public bool TryCollectBulletDamageUpgrade(string pickup_id, int amount)
    {
        if(string.IsNullOrWhiteSpace(pickup_id) || amount <= 0 || IsPermanentPickupCollected(pickup_id))
        {
            return false;
        }

        save_data.collected_permanent_pickup_ids.Add(pickup_id);
        save_data.bullet_damage += amount;
        Save();
        return true;
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(save_data, true);
        File.WriteAllText(SavePath, json);
    }

    public void Load()
    {
        if(!File.Exists(SavePath))
        {
            save_data = new GameSaveData();
            return;
        }

        string json = File.ReadAllText(SavePath);
        save_data = JsonUtility.FromJson<GameSaveData>(json);

        if(save_data == null)
        {
            save_data = new GameSaveData();
            return;
        }
    }
}
