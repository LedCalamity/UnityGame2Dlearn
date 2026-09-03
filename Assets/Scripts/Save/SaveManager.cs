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
        }
    }
}
