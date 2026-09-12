using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public static SceneMgr Instance;
    [SerializeField] string bootstrap_scene_name = "GameBootTrap";

    string selected_level;
    int selected_level_index;
    string pending_menu_panel;

    public int SelectedLevelIndex => selected_level_index;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void StartGame(string level_name, int level_index)
    {
        selected_level = level_name;
        selected_level_index = level_index;
        LoadScene(bootstrap_scene_name);
    }

    public void LoadSelectedLevel()
    {
        LoadScene(selected_level);
    }

    public void LoadScene(string LevelName)
    {
        pending_menu_panel = null;
        StartCoroutine(LoadSceneCoroutine(LevelName));
    }

    public void LoadMainMenu(string panel_name)
    {
        pending_menu_panel = panel_name;
        StartCoroutine(LoadSceneCoroutine("MainMenu"));
    }

    public string TakeMainMenuPanel()
    {
        string panel_name = pending_menu_panel;
        pending_menu_panel = null;
        return panel_name;
    }

    IEnumerator LoadSceneCoroutine(string levelname)
    {
        var asyncLoad = SceneManager.LoadSceneAsync(levelname);
        while(!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
