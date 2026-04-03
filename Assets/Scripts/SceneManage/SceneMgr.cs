using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public static SceneMgr Instance;
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
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
