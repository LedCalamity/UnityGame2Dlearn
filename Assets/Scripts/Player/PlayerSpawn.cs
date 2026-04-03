using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawn : MonoBehaviour
{
    public string[] destroyScenes = {"MainMenu"};
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (destroyScenes.Contains(scene.name))
        {   
            Destroy(gameObject);
            return;
        }
        if (scene.name == "Level1")
        {
            gameObject.GetComponent<PlayerControlDash>().is_unlocked = false;
        }
        GameObject spawnpt = GameObject.FindGameObjectWithTag("SpawnPoint"); //Now it only applies to single spawning point, will improve after learning
        transform.position = spawnpt.transform.position; //only need to create spwan point prefab each scene
    }
}
