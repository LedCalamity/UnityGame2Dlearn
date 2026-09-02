using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public static SceneMgr Instance;
    [SerializeField] string bootstrap_scene_name = "GameBootTrap";

    string selected_level;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void StartGame(string level_name)
    {
        selected_level = level_name;
        LoadScene(bootstrap_scene_name);
    }

    public void LoadSelectedLevel()
    {
        LoadScene(selected_level);
    }

    public void LoadScene(string LevelName)
    {
        StartCoroutine(LoadSceneCoroutine(LevelName));
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
